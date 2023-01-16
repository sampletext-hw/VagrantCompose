using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.Common;
using Models.DTOs.Categories;
using Models.DTOs.Misc;
using Models.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMenuProductRepository _menuProductRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMenuProductRepository menuProductRepository, IMapper mapper, IMenuItemRepository menuItemRepository)
        {
            _categoryRepository = categoryRepository;
            _menuProductRepository = menuProductRepository;
            _mapper = mapper;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<CategoryWithIdDto> GetById(long id)
        {
            var productCategory = await _categoryRepository.GetByIdNonTracking(id);

            var productCategoryDto = _mapper.Map<CategoryWithIdDto>(productCategory);

            return productCategoryDto;
        }

        public async Task<ICollection<CategoryWithIdDto>> GetAll()
        {
            var productCategories = await _categoryRepository.GetAllOrdered();

            var productCategoryDtos = _mapper.Map<ICollection<CategoryWithIdDto>>(productCategories);

            return productCategoryDtos;
        }

        public async Task Update(UpdateCategoryDto updateCategoryDto)
        {
            var productCategory = await _categoryRepository.GetById(updateCategoryDto.Id);

            _mapper.Map(updateCategoryDto, productCategory);

            await _categoryRepository.Update(productCategory);
        }

        public async Task<CreatedDto> Create(CreateCategoryDto createCategoryDto)
        {
            var productCategory = _mapper.Map<Category>(createCategoryDto);

            await _categoryRepository.Add(productCategory);

            return productCategory.Id;
        }

        public async Task Remove(long id)
        {
            var category = await _categoryRepository.GetById(id);

            if (!category.IsDeletable)
            {
                throw new AkianaException("Нельзя удалить эту категорию!");
            }

            var countProducts = await _menuProductRepository.Count(p => p.CategoryId == id);

            if (countProducts != 0)
            {
                throw new AkianaException("Нельзя удалить эту категорию! К ней привязаны продукты!");
            }

            var countItems = await _menuItemRepository.Count(p => p.CategoryId == id);

            if (countItems != 0)
            {
                throw new AkianaException("Нельзя удалить эту категорию! К ней привязаны позиции меню!");
            }

            await _categoryRepository.Remove(category);
        }
    }
}