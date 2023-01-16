namespace Models.DTOs.Misc
{
    public class MessageDto : IDto
    {
        public MessageDto(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}