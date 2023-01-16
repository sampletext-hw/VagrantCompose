using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.Account;
using Models.DTOs.Misc;
using Models.DTOs.WorkerAccountDtos;
using Models.Misc;
using Services.CommonServices.Abstractions;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class WorkerAccountService : IWorkerAccountService
    {
        private readonly IWorkerAccountRepository _workerAccountRepository;

        private readonly IRestaurantRepository _restaurantRepository;

        private readonly IWorkerRoleRepository _workerRoleRepository;

        private readonly ITokenRoleCacheService _tokenRoleStorageService;

        private readonly IMapper _mapper;

        public WorkerAccountService(IWorkerAccountRepository workerAccountRepository, IRestaurantRepository restaurantRepository, IWorkerRoleRepository workerRoleRepository, IMapper mapper, ITokenRoleCacheService tokenRoleStorageService)
        {
            _workerAccountRepository = workerAccountRepository;
            _restaurantRepository = restaurantRepository;
            _workerRoleRepository = workerRoleRepository;
            _mapper = mapper;
            _tokenRoleStorageService = tokenRoleStorageService;
        }

        public async Task<CreatedDto> CreateAccount(CreateWorkerAccountDto createWorkerAccountDto, bool isTechnical = false)
        {
            var accountWithLogin = await _workerAccountRepository.GetOneNonTracking(w => w.Login == createWorkerAccountDto.Login);

            if (accountWithLogin is not null)
            {
                throw new AkianaException("Не удалось создать пользователя. Логин уже занят.");
            }

            var workerAccount = _mapper.Map<WorkerAccount>(createWorkerAccountDto);
            workerAccount.IsTechnical = isTechnical;

            await _workerAccountRepository.Add(workerAccount);

            return new CreatedDto(workerAccount.Id);
        }

        public async Task<WorkerAccountWithIdDto> GetById(long id)
        {
            var workerAccount = await _workerAccountRepository.GetByIdNonTracking(
                id,
                w => w.RestaurantsRelation,
                w => w.WorkerRolesRelation
            );

            var workerAccountWithIdDto = _mapper.Map<WorkerAccountWithIdDto>(workerAccount);

            return workerAccountWithIdDto;
        }

        public async Task<ICollection<WorkerAccountWithIdDto>> GetAll()
        {
            var workerAccounts = await _workerAccountRepository.GetManyNonTracking(
                w => !w.IsTechnical,
                w => w.RestaurantsRelation,
                w => w.WorkerRolesRelation
            );

            var workerAccountsDto = _mapper.Map<ICollection<WorkerAccountWithIdDto>>(workerAccounts);

            return workerAccountsDto;
        }

        public async Task Update(UpdateWorkerAccountDto updateWorkerAccountDto)
        {
            var workerAccount = await _workerAccountRepository.GetById(
                updateWorkerAccountDto.Id,
                w => w.RestaurantsRelation,
                w => w.WorkerRolesRelation.OrderBy(rel => rel.WorkerRoleId)
            );

            if (workerAccount.Login != updateWorkerAccountDto.Login)
            {
                var accountWithLogin = await _workerAccountRepository.GetOne(w => w.Login == updateWorkerAccountDto.Login);

                if (accountWithLogin is not null)
                {
                    throw new AkianaException("Не удалось обновить пользователя. Логин уже занят.");
                }
            }

            if (string.IsNullOrEmpty(updateWorkerAccountDto.Password))
            {
                updateWorkerAccountDto.Password = workerAccount.Password;
            }

            var roleIds = updateWorkerAccountDto.WorkerRoles.Select(dto => dto.Id).OrderBy(id => id).ToList();

            var shouldUpdateRoleStorage = !workerAccount.WorkerRolesRelation.Select(rel => rel.WorkerRoleId).SequenceEqual(roleIds);

            _mapper.Map(updateWorkerAccountDto, workerAccount);

            await _workerAccountRepository.Update(workerAccount);

            if (shouldUpdateRoleStorage)
            {
                await _tokenRoleStorageService.Clear(workerAccount.Id);
            }
        }

        public async Task<CreatedDto> Create(CreateWorkerAccountDto createWorkerAccountDto)
        {
            var workerAccount = _mapper.Map<WorkerAccount>(createWorkerAccountDto);

            await _workerAccountRepository.Add(workerAccount);

            return workerAccount.Id;
        }

        public async Task Remove(long id)
        {
            if (id == 1)
            {
                throw new AkianaException("Нельзя удалить супер-пользователя с Id(1)");
            }

            var workerAccount = await _workerAccountRepository.GetById(id,
                w => w.RestaurantsRelation
            );

            workerAccount.RestaurantsRelation.Clear();
            await _workerAccountRepository.Remove(workerAccount);
        }

        public async Task<ICollection<WorkerAccountWithIdDto>> GetByRestaurant(long id)
        {
            var workerAccounts = await _workerAccountRepository.GetManyNonTracking(
                wa => wa.RestaurantsRelation.Any(r => r.RestaurantId == id),
                w => w.RestaurantsRelation,
                w => w.WorkerRolesRelation
            );

            var workerAccountDtos = _mapper.Map<ICollection<WorkerAccountWithIdDto>>(workerAccounts);
            return workerAccountDtos;
        }
    }
}