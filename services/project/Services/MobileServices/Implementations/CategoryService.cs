using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.Categories;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<CategoryMobileDto>> GetAll()
        {
            var productCategories = await _categoryRepository.GetAllOrdered();

            var productCategoryDtos = _mapper.Map<ICollection<CategoryMobileDto>>(productCategories);

            return productCategoryDtos;
        }
    }
}