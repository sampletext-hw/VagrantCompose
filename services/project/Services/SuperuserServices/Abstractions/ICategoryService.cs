using Models.DTOs.Categories;

namespace Services.SuperuserServices.Abstractions
{
    public interface ICategoryService : ICrudService<CategoryWithIdDto, CreateCategoryDto, UpdateCategoryDto>
    {
    }
}