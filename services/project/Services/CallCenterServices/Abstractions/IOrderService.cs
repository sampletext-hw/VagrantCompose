using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Misc;
using Models.DTOs.Orders;

namespace Services.CallCenterServices.Abstractions
{
    public interface IOrderService
    {
        Task<OrderDto> GetById(long id);

        Task<ICollection<OrderDto>> GetByDateRange(DateTime left, DateTime right);
    }
}