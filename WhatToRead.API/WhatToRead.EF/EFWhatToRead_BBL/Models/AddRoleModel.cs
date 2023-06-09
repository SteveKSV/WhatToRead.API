using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Models
{
    public class AddRoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
