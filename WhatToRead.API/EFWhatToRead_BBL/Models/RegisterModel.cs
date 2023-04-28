using System.ComponentModel.DataAnnotations;

namespace EFWhatToRead_BBL.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "Email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
