using Models.DTOs.PriceGroups;

namespace Services.SuperuserServices.Abstractions
{
    using TWithIdDto = PriceGroupWithIdDto;
    using TCreateDto = CreatePriceGroupDto;
    using TUpdateDto = UpdatePriceGroupDto;

    public interface IPriceGroupService : ICrudService<TWithIdDto, TCreateDto, TUpdateDto>
    {
    }
}