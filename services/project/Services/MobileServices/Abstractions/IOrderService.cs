using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Misc;
using Models.DTOs.Orders;

namespace Services.MobileServices.Abstractions
{
    public interface IOrderService
    {
        Task<OrderMobileDto> GetById(long id);
        
        Task<CreatedDto> CreateFromCart(MobileCreateOrderFromCartDto mobileCreateOrderFromCartDto);
        Task<OrderCreationResultDto> CreateFromCartV2(MobileCreateOrderFromCartV2Dto mobileCreateOrderFromCartDto);

        Task<ICollection<OrderMobileDto>> GetByClient(long id, int offset = 0, int limit = 25);
        
        Task<ICollection<OrderMobileDto>> GetByAddress(long id);
        
        Task<ICollection<OrderMobileDto>> GetByClientAndRestaurant(long clientId, long restaurantId);
        Task TryConfirmOrderPayment(long orderId);
    }
}