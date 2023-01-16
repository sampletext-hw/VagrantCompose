namespace Models.DTOs.Misc
{
    public class ErrorDto : IDto
    {
        public ErrorDto(string error)
        {
            Error = error;
        }

        public string Error { get; set; }
    }
}