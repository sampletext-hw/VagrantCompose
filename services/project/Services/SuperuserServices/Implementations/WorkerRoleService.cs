using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Infrastructure.Verbatims;
using Models.DTOs.WorkerRoles;
using Models.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class WorkerRoleService : IWorkerRoleService
    {
        private readonly IWorkerAccountRepository _workerAccountRepository;

        private readonly IWorkerRoleRepository _workerRoleRepository;

        private readonly IMapper _mapper;

        public WorkerRoleService(IWorkerAccountRepository workerAccountRepository, IWorkerRoleRepository workerRoleRepository, IMapper mapper)
        {
            _workerAccountRepository = workerAccountRepository;
            _workerRoleRepository = workerRoleRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<WorkerRoleDto>> GetAll()
        {
            var workerRoles = await _workerRoleRepository.GetManyNonTracking();

            var workerRoleDtos = _mapper.Map<ICollection<WorkerRoleDto>>(workerRoles.Where(
                    r => r.TitleEn != VRoles.AndroidApplication && r.TitleEn != VRoles.IPhoneApplication
                )
            );

            return workerRoleDtos;
        }

        public async Task AddToRole(long workerId, long roleId)
        {
            var workerAccount = await _workerAccountRepository.GetById(workerId, w => w.WorkerRolesRelation);

            workerAccount.EnsureNotNullHandled(VMessages.AccountNotFound);

            if (workerAccount.WorkerRolesRelation.Any(r => r.WorkerRoleId == roleId))
            {
                throw new AkianaException("У этого пользователя уже есть выбранная роль.");
            }

            var role = await _workerRoleRepository.GetById(roleId);

            role.EnsureNotNullHandled("Role not found");

            workerAccount.WorkerRolesRelation.Add(new()
            {
                WorkerAccountId = workerAccount.Id,
                WorkerRoleId = roleId
            });

            await _workerAccountRepository.Update(workerAccount);
        }

        public async Task AddToRole(long workerId, string roleTitleEn)
        {
            var role = await _workerRoleRepository.GetByTitleEn(roleTitleEn);

            role.EnsureNotNullHandled("Role not found");

            await AddToRole(workerId, role.Id);
        }

        public async Task RemoveFromRole(long workerId, long roleId)
        {
            var workerAccount = await _workerAccountRepository.GetById(workerId, wa => wa.WorkerRolesRelation);

            workerAccount.EnsureNotNullHandled(VMessages.AccountNotFound);

            var relation = workerAccount.WorkerRolesRelation.FirstOrDefault(r => r.WorkerRoleId == roleId);

            relation.EnsureNotNullHandled("Account is not in role");

            workerAccount.WorkerRolesRelation.Remove(relation);

            await _workerAccountRepository.Update(workerAccount);
        }

        public async Task RemoveFromRole(long workerId, string roleTitleEn)
        {
            var role = await _workerRoleRepository.GetByTitleEn(roleTitleEn);

            role.EnsureNotNullHandled("Role not found");

            await RemoveFromRole(workerId, role.Id);
        }

        public async Task<GetRolesResultDto> GetByWorker(long id)
        {
            var workerAccount = await _workerAccountRepository.GetByIdNonTracking(id, wa => wa.WorkerRoles);

            var roleDtos = _mapper.Map<ICollection<WorkerRoleDto>>(workerAccount.WorkerRoles);

            return new GetRolesResultDto(roleDtos);
        }
    }
}