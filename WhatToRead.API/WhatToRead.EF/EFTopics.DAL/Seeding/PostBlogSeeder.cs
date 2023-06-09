using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Seeding;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.BBL.Seeding
{
    public class PostBlogSeeder : ISeeder<PostBlog>
    {
      
        private readonly List<PostBlog> postBlogs = new List<PostBlog>()
        {
            new PostBlog
            {
                PostId = 3,
                TopicId = 1
            },
            new PostBlog
            {
                PostId = 4,
                TopicId = 1,
            },
            new PostBlog
            {
                PostId = 5,
                TopicId = 2,
            }
            
        };
        public void Seed(EntityTypeBuilder<PostBlog> builder)
        {
            builder.HasData(postBlogs);
        }
    }
}
