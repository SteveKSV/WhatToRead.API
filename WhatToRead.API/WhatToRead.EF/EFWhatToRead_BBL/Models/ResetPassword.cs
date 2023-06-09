using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Models
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
