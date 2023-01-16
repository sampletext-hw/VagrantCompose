using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Categories;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class CategoryController : AkianaMobileController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает список категорий меню (с шифрацией)")]
        public async Task<ActionResult<ICollection<CategoryMobileDto>>> GetAll()
        {
            var categoryMobileDtos = await _categoryService.GetAll();
            return Ok(categoryMobileDtos);
        }
    }
}