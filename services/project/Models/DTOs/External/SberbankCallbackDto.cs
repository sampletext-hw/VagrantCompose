namespace Models.DTOs.External;

public class SberbankCallbackDto
{
    public int OrderNumber { get; set; }

    public string MdOrder { get; set; }

    public string Operation { get; set; }

    public int Status { get; set; }
}