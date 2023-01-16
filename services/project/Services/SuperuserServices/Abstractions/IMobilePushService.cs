using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Misc;
using Models.DTOs.MobilePushes;

namespace Services.SuperuserServices.Abstractions
{
    public interface IMobilePushService
    {
        Task<MobilePushWithIdDto> GetByIdByCities(long id);

        Task<MobilePushWithIdDto> GetByIdByPriceGroups(long id);

        Task<ICollection<MobilePushWithIdDto>> GetAllByCities();

        Task<ICollection<MobilePushWithIdDto>> GetAllByPriceGroups();

        Task DeleteByCities(long id);
        Task DeleteByPriceGroups(long id);

        Task<CreatedDto> CreateByCities(CreateMobilePushDto createMobilePushDto, bool send = true);

        Task<CreatedDto> CreateByPriceGroups(CreateMobilePushDto createMobilePushDto, bool send = true);
    }
}