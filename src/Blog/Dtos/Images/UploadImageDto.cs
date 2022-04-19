using System.ComponentModel.DataAnnotations;

namespace Blog.Dtos.Images
{
    public class UploadImageDto
    {
        [Required]
        public string Base64Image { get; set; }
    }
}
