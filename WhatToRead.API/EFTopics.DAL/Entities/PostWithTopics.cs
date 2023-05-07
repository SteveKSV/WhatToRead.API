using EFTopics.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Entities
{
    public class PostWithTopics
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Views { get; set; }
        public string Image { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
