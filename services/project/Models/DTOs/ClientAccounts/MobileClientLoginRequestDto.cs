using Models.DTOs.Misc;

namespace Models.DTOs.ClientAccounts
{
    public class MobileClientLoginRequestDto : IDto
    {
        public long Id { get; set; }
        public CodeSendType SendType { get; set; }
    }
}