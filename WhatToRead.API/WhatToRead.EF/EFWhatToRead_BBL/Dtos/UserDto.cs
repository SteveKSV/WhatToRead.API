using EFWhatToRead_BBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public enumUser Role { get; set; }
    }
    public enum enumUser
    {
        Admin,
        User
    }
}
