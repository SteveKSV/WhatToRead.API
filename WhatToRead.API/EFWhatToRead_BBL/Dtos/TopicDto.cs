using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Dtos
{
    public class TopicDto
    {
        public int TopicId { get; set; }
        [Required(ErrorMessage = "The Title field is required.")]
        public string Name { get; set; }
    }
}
