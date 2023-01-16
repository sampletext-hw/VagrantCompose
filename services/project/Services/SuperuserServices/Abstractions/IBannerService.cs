using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Banners;
using Models.DTOs.General;
using Models.DTOs.Misc;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = BannerWithIdDto;
    using TCreateDto = CreateBannerDto;
    using TUpdateDto = UpdateBannerDto;

    public interface IBannerService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
        Task<ICollection<TWithIdDto>> GetByCity(long id);
        
        Task<IsActiveDto> ToggleActive(long id);
    }
}