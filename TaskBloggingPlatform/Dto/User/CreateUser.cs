using System.ComponentModel.DataAnnotations;

namespace TaskBloggingPlatform.Dto.User
{
    public class CreateUser
    {
        // Username
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters")]
        public string UserName { get; set; }

        // Password
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; }

        // Email
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
