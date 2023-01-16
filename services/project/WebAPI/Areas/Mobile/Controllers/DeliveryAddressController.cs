using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.Attributes;
using Models.Db;
using Models.Db.Account;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Misc;
using Services.MobileServices.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using WebAPI.Filters;

namespace WebAPI.Areas.Mobile.Controllers
{
    public class DeliveryAddressController : AkianaMobileController
    {
        private readonly IDeliveryAddressService _deliveryAddressService;

        public DeliveryAddressController(IDeliveryAddressService deliveryAddressService)
        {
            _deliveryAddressService = deliveryAddressService;
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Добавляет новый адрес доставки пользователя (с шифрацией)")]
        public async Task<ActionResult<CreatedDto>> Add([ModelBinder(typeof(EncodedJsonBinder))] AddDeliveryAddressDto addDeliveryAddressDto)
        {
            var createdDto = await _deliveryAddressService.Add(addDeliveryAddressDto);

            return Ok(createdDto);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [SwaggerOperation("Обновляет адрес доставки пользователя (с шифрацией)")]
        public async Task<ActionResult> Update([ModelBinder(typeof(EncodedJsonBinder))] UpdateDeliveryAddressDto updateDeliveryAddressDto)
        {
            await _deliveryAddressService.Update(updateDeliveryAddressDto);

            return Ok();
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(SerializeOutputFilter))]
        [SwaggerOperation("Получает все адреса доставки пользователя (с шифрацией)")]
        public async Task<ActionResult<ICollection<DeliveryAddressMobileDto>>> GetByClient([Id(typeof(ClientAccount))] long id)
        {
            var deliveryAddressMobileDtos = await _deliveryAddressService.GetByClient(id);

            return Ok(deliveryAddressMobileDtos);
        }
        
        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.IPhoneApplication, VRoles.AndroidApplication)]
        [TypeFilter(typeof(RequireFullAccessFilter))]
        [SwaggerOperation("Удаляет указанный адрес доставки (с шифрацией)")]
        public async Task<ActionResult> Delete([Id(typeof(DeliveryAddress))] long id)
        {
            await _deliveryAddressService.Delete(id);

            return Ok();
        }
    }
}