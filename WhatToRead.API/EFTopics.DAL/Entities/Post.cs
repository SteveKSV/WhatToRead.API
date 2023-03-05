using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EFTopics.DAL.Entities
{
    public class Post
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


        public ICollection<PostBlog> PostBlogs { get; set; }
    }
}
