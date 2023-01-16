using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.MenuItems;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<MenuItemMobileDto>> GetByPriceGroup(long id)
        {
            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => item.PriceGroupsRelation.Any(r => r.PriceGroupId == id),
                item => item.CPFC,
                // here we load price for selected price group
                item => item.PriceGroupsRelation.Where(p => p.PriceGroupId == id),
                item => item.MenuProductsRelation,
                item => item.Measures
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemMobileDto>>(menuItems);

            return menuItemDtos;
        }

        public async Task<ICollection<MenuItemMobileDto>> GetByCategory(long id)
        {
            var menuItems = await _menuItemRepository.GetMany(
                item => item.CategoryId == id,
                item => item.CPFC,
                // here we load price for selected price group
                item => item.PriceGroupsRelation.Where(p => p.PriceGroupId == id),
                item => item.MenuProductsRelation,
                item => item.Measures
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemMobileDto>>(menuItems);

            return menuItemDtos;
        }
    }
}