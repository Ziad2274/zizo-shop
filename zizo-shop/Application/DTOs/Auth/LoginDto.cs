using System.ComponentModel.DataAnnotations;

namespace zizo_shop.Application.DTOs.User
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
