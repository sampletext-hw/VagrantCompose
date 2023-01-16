using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs.Misc;

namespace Services.SuperuserServices
{
    public interface ICrudService<TWithIdDto, in TCreateDto, in TUpdateDto>
    {
        Task<TWithIdDto> GetById(long id);

        Task<ICollection<TWithIdDto>> GetAll();

        Task Update(TUpdateDto updateDto);
        
        Task<CreatedDto> Create(TCreateDto createDto);

        Task Remove(long id);
    }
}