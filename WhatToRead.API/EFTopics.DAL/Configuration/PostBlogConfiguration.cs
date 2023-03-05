using EFTopics.DAL.Entities;
using EFTopics.DAL.Seeding;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
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
            builder
                .HasKey(x => new { x.PostId, x.TopicId });
        }
    }
}
