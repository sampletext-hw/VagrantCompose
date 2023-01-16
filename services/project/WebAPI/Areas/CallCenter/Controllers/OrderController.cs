using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db.DbOrder;
using Models.DTOs.Misc;
using Models.DTOs.Orders;
using Models.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services;
using Services.CallCenterServices.Abstractions;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;
using WebAPI.Filters;

namespace WebAPI.Areas.CallCenter.Controllers
{
    public class OrderController : AkianaCallCenterController
    {
        private readonly IOrderService _orderService;

        private readonly ISSEService _sseService;

        private readonly JsonSerializerSettings _jsonSettings;

        public OrderController(IOrderService orderService, ISSEService sseService)
        {
            _orderService = orderService;
            _sseService = sseService;
            _jsonSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
        }

        // Note: JS EventSource doesn't support custom headers, so disable AuthTokenFilter here 
        [HttpGet]
        //[TypeFilter(typeof(AuthTokenFilter))]
        public void Sse()
        {
            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("X-Accel-Buffering", "no");

            async void OnOrderCreated(long id)
            {
                try
                {
                    await TelegramAPI.Send($"CallCenter/Order/SSE\nSent {id}");
                    var messageJson = JsonConvert.SerializeObject(new CreatedDto(id), _jsonSettings);

                    await Response.WriteAsync($"data: {messageJson}\n");
                    await Response.WriteAsync($"id: {id}\n\n");
                    await Response.Body.FlushAsync();
                }
                catch (Exception e)
                {
                    await TelegramAPI.Send($"CallCenter/Order/SSE failed in OnOrderCreated.\n{e.ToPrettyString()}");
                }
            }

            _sseService.OrderCreated += OnOrderCreated;

            // _logger.LogInformation("SSE connected");

            string lastEventIdString = Request.Headers["Last-Event-ID"];

            if (int.TryParse(lastEventIdString, out var lastEventId))
            {
                for (var i = lastEventId; i < _sseService.LastOrderId; i++)
                {
                    OnOrderCreated(i);
                }
            }

            HttpContext.RequestAborted.WaitHandle.WaitOne();

            _sseService.OrderCreated -= OnOrderCreated;

            // _logger.LogInformation("SSE disconnected");
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.CallCenter)]
        public async Task<ActionResult<OrderDto>> GetById([Id(typeof(Order))] long id)
        {
            var orderDto = await _orderService.GetById(id);
            return Ok(orderDto);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.CallCenter)]
        public async Task<ActionResult<ICollection<OrderDto>>> GetByDateRange(
            [Required] DateTime left,
            [Required] DateTime right
        )
        {
            var orderDtos = await _orderService.GetByDateRange(left, right);
            return Ok(orderDtos);
        }
    }
}