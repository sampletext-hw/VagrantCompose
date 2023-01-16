using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.MenuProducts;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
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

        public async Task<ICollection<MenuProductMobileDto>> GetByMenuItem(long id)
        {
            var menuProducts = await _menuProductRepository.GetManyNonTracking(
                p => p.MenuItemsRelation.Any(i => i.MenuItemId == id),
                p => p.Category
            );

            var menuProductWithIdDtos = _mapper.Map<ICollection<MenuProductMobileDto>>(menuProducts);

            return menuProductWithIdDtos;
        }

        public async Task<ICollection<MenuProductMobileDto>> GetByMenuItems(ICollection<long> ids)
        {
            var menuProducts = await _menuProductRepository.GetManyNonTracking(
                p => p.MenuItemsRelation.Any(i => ids.Contains(i.MenuItemId)),
                p => p.Category
            );
            
            var menuProductWithIdDtos = _mapper.Map<ICollection<MenuProductMobileDto>>(menuProducts);

            return menuProductWithIdDtos;
        }
    }
}