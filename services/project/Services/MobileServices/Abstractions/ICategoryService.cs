using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Categories;

namespace Services.MobileServices.Abstractions
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryMobileDto>> GetAll();
    }
}