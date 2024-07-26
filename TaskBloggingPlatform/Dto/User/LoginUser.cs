using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.User
{
    public class LoginUser
    {
        // Username
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        // Password
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
