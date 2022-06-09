using System.ComponentModel.DataAnnotations;

namespace moneyboss_user_service.Models
{
    public class UserRegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        [Required, MinLength(4)]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
