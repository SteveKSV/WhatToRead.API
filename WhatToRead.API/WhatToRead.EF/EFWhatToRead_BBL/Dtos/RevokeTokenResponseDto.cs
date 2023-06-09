using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Dtos
{
    public class RevokeTokenResponseDto
    {
        public Boolean isSucceeded { get; set; }
        public string Message { get; set; }
    }
}
