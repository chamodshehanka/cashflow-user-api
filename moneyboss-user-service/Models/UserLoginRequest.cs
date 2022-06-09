using System.ComponentModel.DataAnnotations;

namespace moneyboss_user_service.Models
{
    public class UserLoginRequest
    {
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
