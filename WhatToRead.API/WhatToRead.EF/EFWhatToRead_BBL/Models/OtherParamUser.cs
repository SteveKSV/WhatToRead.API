using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Models
{
    public class OtherParamUser : RegisterModel
    {
        public string UserName { get; set; }

        [Required]
        public enumUser RoleUser {get; set;}
    }

    public enum enumUser
    {
        Admin, 
        User
    }
}
