using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs;
using Models.DTOs.Misc;
using Models.DTOs.WorkerAccountDtos;
using Models.DTOs.WorkerRoles;

namespace Services.SuperuserServices.Abstractions
{
    public interface IWorkerAccountService : ICrudService<WorkerAccountWithIdDto, CreateWorkerAccountDto, UpdateWorkerAccountDto>
    {
        Task<CreatedDto> CreateAccount(CreateWorkerAccountDto createWorkerAccountDto, bool isTechnical = false);

        Task<ICollection<WorkerAccountWithIdDto>> GetByRestaurant(long id);
    }
}