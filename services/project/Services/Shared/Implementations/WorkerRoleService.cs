using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.WorkerRoles;
using Services.Shared.Abstractions;

namespace Services.Shared.Implementations
{
    public class WorkerRoleService : IWorkerRoleService
    {
        private readonly IWorkerAccountRepository _workerAccountRepository;

        private readonly IWorkerRoleRepository _workerRoleRepository;

        private readonly IMapper _mapper;

        private readonly IRequestAccountIdService _requestAccountIdService;

        public WorkerRoleService(IWorkerAccountRepository workerAccountRepository, IWorkerRoleRepository workerRoleRepository, IMapper mapper, IRequestAccountIdService requestAccountIdService)
        {
            _workerAccountRepository = workerAccountRepository;
            _workerRoleRepository = workerRoleRepository;
            _mapper = mapper;
            _requestAccountIdService = requestAccountIdService;
        }

        public async Task<GetRolesResultDto> GetByWorker(long id)
        {
            var workerAccount = await _workerAccountRepository.GetByIdNonTracking(id, wa => wa.WorkerRoles);

            var roleDtos = _mapper.Map<ICollection<WorkerRoleDto>>(workerAccount.WorkerRoles);

            return new GetRolesResultDto(roleDtos);
        }

        public async Task<GetRolesResultDto> GetMy()
        {
            var workerAccount = await _workerAccountRepository.GetByIdNonTracking(
                _requestAccountIdService.Id,
                wa => wa.WorkerRoles
            );

            var roleDtos = _mapper.Map<ICollection<WorkerRoleDto>>(workerAccount.WorkerRoles);

            return new GetRolesResultDto(roleDtos);
        }
    }
}