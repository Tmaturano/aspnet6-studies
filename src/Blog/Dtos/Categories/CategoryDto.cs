using System.ComponentModel.DataAnnotations;

namespace Blog.Dtos.Categories
{
    public abstract class CategoryDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public string Slug { get; set; }
    }
}
