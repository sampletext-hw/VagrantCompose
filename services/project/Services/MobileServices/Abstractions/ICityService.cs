using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Cities;

namespace Services.MobileServices.Abstractions
{
    public interface ICityService
    {
        Task<ICollection<CityMobileDto>> GetAll();
    }
}