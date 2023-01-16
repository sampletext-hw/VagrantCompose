using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.DbOrder;
using Models.DTOs.Orders;
using Models.Misc;
using Services.CallCenterServices.Abstractions;

namespace Services.CallCenterServices.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICityRepository _cityRepository;

        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICityRepository cityRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetById(long id)
        {
            var order = await _orderRepository.GetByIdForCallCenter(id);

            var gmtOffset = await _cityRepository.GetGmtOffsetByOrder(order.Id);

            order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffset));
            order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);

            var orderDto = _mapper.Map<OrderDto>(order);

            return orderDto;
        }

        public async Task<ICollection<OrderDto>> GetByDateRange(DateTime left, DateTime right)
        {
            var orders = await _orderRepository.GetByDateRangeForCallCenter(left, right);

            var gmtOffsetsByOrders = await _cityRepository.GetGmtOffsetsByOrders(orders.Select(o => o.Id).ToList());

            foreach (var order in orders)
            {
                order.CreatedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.CreatedAtDateTime.AddHours(gmtOffsetsByOrders[order.Id]));
                order.AwaitedAtDateTime = TimeZoneInfo.ConvertTimeToUtc(order.AwaitedAtDateTime);
            }

            var orderDtos = _mapper.Map<ICollection<OrderDto>>(orders);

            return orderDtos;
        }
    }
}