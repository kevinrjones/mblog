using System.ComponentModel.DataAnnotations;

namespace MBlog.Models.User
{
    public class LoginUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}