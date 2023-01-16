using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Banners;

namespace Services.MobileServices.Abstractions
{
    public interface IBannerService
    {
        Task<ICollection<BannerMobileDto>> GetByCity(long id);
    }
}