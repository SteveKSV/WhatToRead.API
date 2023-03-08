using EFTopics.DAL.Entities;
using EFTopics.DAL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.DAL.Configuration
{
    public class PostBlogConfiguration : IEntityTypeConfiguration<PostBlog>
    {
        public void Configure(EntityTypeBuilder<PostBlog> builder)
        {
            new PostBlogSeeder().Seed(builder);
        }
    }
}
