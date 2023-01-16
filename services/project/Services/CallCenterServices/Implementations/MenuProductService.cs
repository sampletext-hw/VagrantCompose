using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;
using Services.CallCenterServices.Abstractions;

namespace Services.CallCenterServices.Implementations
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
    }
}