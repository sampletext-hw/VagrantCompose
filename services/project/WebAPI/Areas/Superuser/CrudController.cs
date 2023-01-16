using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Verbatims;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Misc;
using Services.SuperuserServices;
using WebAPI.Filters;

namespace WebAPI.Areas.Superuser
{
    public abstract class CrudController<TBase, TWithIdDto, TCreateDto, TUpdateDto> : AkianaSuperuserController
    {
        private readonly ICrudService<TWithIdDto, TCreateDto, TUpdateDto> _service;
        private readonly Func<Type, object, bool> _existenceChecker;

        private readonly Type _type = typeof(TBase);
        
        protected CrudController(ICrudService<TWithIdDto, TCreateDto, TUpdateDto> service, Func<Type, object, bool> existenceChecker)
        {
            _service = service;
            _existenceChecker = existenceChecker;
        }

        [NonAction]
        private Task EnsureExists(long id)
        {
            var exists = _existenceChecker.Invoke(_type, id);
            if (!exists)
            {
                throw new($"{_type} with Id({id}) not found");
            }

            return Task.CompletedTask;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        public async Task<ActionResult<TWithIdDto>> GetById(long id)
        {
            await EnsureExists(id);
            return await _service.GetById(id);
        }

        [HttpGet]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<ICollection<TWithIdDto>>> GetAll()
        {
            var withIdDtos = await _service.GetAll();
            return Ok(withIdDtos);
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult> Update([FromBody] TUpdateDto updateDto)
        {
            await _service.Update(updateDto);
            return Ok();
        }

        [HttpPost]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult<CreatedDto>> Create([FromBody] TCreateDto createDto)
        {
            var createdDto = await _service.Create(createDto);
            return createdDto;
        }

        [HttpDelete]
        [TypeFilter(typeof(AuthTokenFilter))]
        [RolesFilter(VRoles.Superuser)]
        public async Task<ActionResult> Remove(long id)
        {
            await EnsureExists(id);
            await _service.Remove(id);
            return Ok();
        }
    }
}