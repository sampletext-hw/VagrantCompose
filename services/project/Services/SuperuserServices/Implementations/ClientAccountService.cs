using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using Models.Db.Account;
using Models.DTOs;
using Models.DTOs.ClientAccounts;
using Models.DTOs.Misc;
using Services.SuperuserServices.Abstractions;

namespace Services.SuperuserServices.Implementations
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IClientAccountRepository _clientAccountRepository;

        private readonly IMapper _mapper;

        public ClientAccountService(IClientAccountRepository clientAccountRepository, IMapper mapper)
        {
            _clientAccountRepository = clientAccountRepository;
            _mapper = mapper;
        }

        public async Task<ClientAccountDto> GetById(long id)
        {
            var clientAccount = await _clientAccountRepository.GetByIdNonTracking(id);

            var clientAccountDto = _mapper.Map<ClientAccountDto>(clientAccount);

            return clientAccountDto;
        }
    }
}