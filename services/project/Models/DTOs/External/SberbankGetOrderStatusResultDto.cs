namespace Models.DTOs.External;

public class SberbankGetOrderStatusResultDto
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public int OrderStatus { get; set; }
}