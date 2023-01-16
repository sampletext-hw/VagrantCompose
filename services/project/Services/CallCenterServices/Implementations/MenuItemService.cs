using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.MenuItems;
using Models.DTOs.Misc;
using Services.CallCenterServices.Abstractions;

namespace Services.CallCenterServices.Implementations
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

        public async Task<ICollection<MenuItemWithIdDto>> GetMany(IdsDto idsDto)
        {
            var ids = idsDto.Ids.Select(d => d.Id);

            var menuItems = await _menuItemRepository.GetManyNonTracking(
                item => ids.Contains(item.Id),
                item => item.CPFC,
                item => item.PriceGroups.OrderBy(p => p.Id),
                item => item.MenuProductsRelation,
                item => item.Measures
            );

            var menuItemDtos = _mapper.Map<ICollection<MenuItemWithIdDto>>(menuItems);

            return menuItemDtos;
        }
    }
}