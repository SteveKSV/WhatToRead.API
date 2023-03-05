using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EFTopics.DAL.Entities
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        

        public ICollection<PostBlog> PostBlogs { get; set; }
    }
}
