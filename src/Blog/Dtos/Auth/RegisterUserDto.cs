using System.ComponentModel.DataAnnotations;

namespace Blog.Dtos.Auth
{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int? Role { get; set; }
    }
}
