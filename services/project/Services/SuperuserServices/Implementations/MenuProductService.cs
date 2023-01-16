using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.Menu;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;
using Models.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class MenuProductService : IMenuProductService
    {
        private readonly IMenuProductRepository _menuProductRepository;

        private readonly IMapper _mapper;

        public MenuProductService(IMenuProductRepository menuProductRepository, IMapper mapper)
        {
            _menuProductRepository = menuProductRepository;
            _mapper = mapper;
        }

        public async Task<CreatedDto> Create(CreateMenuProductDto createMenuProductDto)
        {
            var menuProduct = _mapper.Map<MenuProduct>(createMenuProductDto);

            await _menuProductRepository.Add(menuProduct);

            return menuProduct.Id;
        }

        public async Task<MenuProductWithIdDto> GetById(long id)
        {
            var menuProduct = await _menuProductRepository.GetByIdNonTracking(id, p => p.Category);

            var menuProductGetByIdResultDto = _mapper.Map<MenuProductWithIdDto>(menuProduct);

            return menuProductGetByIdResultDto;
        }

        public async Task<ICollection<MenuProductWithIdDto>> GetAll()
        {
            var menuProducts = await _menuProductRepository.GetManyNonTracking(null, p => p.Category);

            var menuProductWithIdDtos = _mapper.Map<ICollection<MenuProductWithIdDto>>(menuProducts);

            return menuProductWithIdDtos;
        }

        public async Task<ICollection<MenuProductWithIdDto>> GetByMenuItem(long id)
        {
            var menuProducts = await _menuProductRepository.GetManyNonTracking(
                p => p.MenuItemsRelation.Any(r => r.MenuItemId == id),
                p => p.Category
            );

            var menuProductWithIdDtos = _mapper.Map<ICollection<MenuProductWithIdDto>>(menuProducts);

            return menuProductWithIdDtos;
        }

        public async Task<ICollection<MenuProductWithIdDto>> GetByCategory(long id)
        {
            var menuProducts = await _menuProductRepository.GetManyNonTracking(
                p => p.CategoryId == id,
                p => p.Category
            );

            var menuProductWithIdDtos = _mapper.Map<ICollection<MenuProductWithIdDto>>(menuProducts);

            return menuProductWithIdDtos;
        }

        public async Task<ICollection<MenuProductWithIdDto>> GetMany(IdsDto idsDto)
        {
            var ids = idsDto.Ids.Select(d => d.Id);

            var menuProducts = await _menuProductRepository.GetManyNonTracking(
                p => ids.Contains(p.Id),
                p => p.Category
            );

            var menuProductDtos = _mapper.Map<ICollection<MenuProductWithIdDto>>(menuProducts);

            return menuProductDtos;
        }

        public async Task Update(UpdateMenuProductDto updateMenuProductDto)
        {
            var menuProduct = await _menuProductRepository.GetById(updateMenuProductDto.Id);

            _mapper.Map(updateMenuProductDto, menuProduct);

            await _menuProductRepository.Update(menuProduct);
        }

        public async Task Remove(long id)
        {
            var menuProduct = await _menuProductRepository.GetById(id, p => p.MenuItems);

            if (menuProduct.MenuItems.Count > 0)
            {
                throw new AkianaException("Нельзя удалить этот Продукт! Он задействован в меню.");
            }

            await _menuProductRepository.Remove(menuProduct);
        }
    }
}