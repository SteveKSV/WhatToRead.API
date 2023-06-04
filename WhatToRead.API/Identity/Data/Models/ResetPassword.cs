using System.ComponentModel.DataAnnotations;

namespace Identity.Data.Models
{
    public class ResetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string NewPassword { get; set; }
    }
}
