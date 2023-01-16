using System;
using System.Threading.Tasks;
using Models.Db.Common;
using Models.DTOs.Categories;
using Services.SuperuserServices.Abstractions;

namespace WebAPI.Areas.Superuser.Controllers
{
    public class CategoryController : CrudController<Category, CategoryWithIdDto, CreateCategoryDto, UpdateCategoryDto>
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, Func<Type, object, bool> existenceChecker) : base(categoryService, existenceChecker)
        {
            _categoryService = categoryService;
        }
    }
}