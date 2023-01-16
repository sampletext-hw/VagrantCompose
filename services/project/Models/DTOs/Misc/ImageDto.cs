namespace Models.DTOs.Misc
{
    public class ImageDto : IDto
    {
        public string Image { get; set; }

        public ImageDto(string image)
        {
            Image = image;
        }
    }
}