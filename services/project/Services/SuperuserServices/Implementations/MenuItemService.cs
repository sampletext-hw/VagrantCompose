using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.Menu;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMenuProductRepository _menuProductRepository;

        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper, IMenuProductRepository menuProductRepository)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
            _menuProductRepository = menuProductRepository;
        }

        public async Task<CreatedDto> Create(CreateMenuItemDto createMenuItemDto)
        {
            var menuItem = _mapper.Map<MenuItem>(createMenuItemDto);

            await _menuItemRepository.Add(menuItem);

            return menuItem.Id;
        }

        public async Task<MenuItemWithIdDto> GetById(long id)
        {
            var menuItem = await _menuItemRepository.GetByIdNonTracking(
                id,
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDto = _mapper.Map<MenuItemWithIdDto>(menuItem);

            return menuItemDto;
        }

        public async Task<ICollection<MenuItemWithIdDto>> GetAll()
        {
            var menuItems = await _menuItemRepository.GetManyNonTracking(
                null,
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task<ICollection<MenuItemWithIdDto>> GetByPriceGroup(long id)
        {
            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => item.PriceGroupsRelation.Any(r => r.PriceGroupId == id),
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task<ICollection<MenuItemWithIdDto>> GetByMenuProduct(long id)
        {
            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => item.MenuProductsRelation.Any(r => r.MenuProductId == id),
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task<ICollection<MenuItemWithIdDto>> GetByCategory(long id)
        {
            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => item.CategoryId == id,
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task<ICollection<MenuItemWithIdDto>> GetMany(IdsDto idsDto)
        {
            var ids = idsDto.Ids.Select(d => d.Id);

            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => ids.Contains(item.Id),
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures,
                item => item.Category
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task Update(UpdateMenuItemDto updateMenuItemDto)
        {
            var menuItem = await _menuItemRepository.GetById(
                updateMenuItemDto.Id,
                item => item.CPFC,
                item => item.PriceGroupsRelation,
                item => item.MenuProductsRelation,
                item => item.Measures
            );

            menuItem.MenuProductsRelation.Clear();
            menuItem.PriceGroupsRelation.Clear();

            await _menuItemRepository.Update(menuItem);

            _mapper.Map(updateMenuItemDto, menuItem);

            await _menuItemRepository.Update(menuItem);
        }

        public async Task Remove(long id)
        {
            var menuItem = await _menuItemRepository.GetById(id);

            await _menuItemRepository.Remove(menuItem);
        }
    }
}