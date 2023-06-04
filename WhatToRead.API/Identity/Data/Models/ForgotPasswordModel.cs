using System.ComponentModel.DataAnnotations;

namespace Identity.Data.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
