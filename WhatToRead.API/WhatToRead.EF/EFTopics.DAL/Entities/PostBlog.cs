using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EFTopics.BBL.Entities
{
    public class PostBlog
    { 
        public int PostId { get; set; }
        public int TopicId { get; set; }


        public Post Post { get; set; }
  
        public Topic Topic { get; set; }
    }
}
