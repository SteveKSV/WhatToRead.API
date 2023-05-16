using EFTopics.BBL.Entities;
using EFTopics.BBL.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Data.Configuration
{
    public class PostBlogConfiguration : IEntityTypeConfiguration<PostBlog>
    {
        public void Configure(EntityTypeBuilder<PostBlog> builder)
        {
            new PostBlogSeeder().Seed(builder);
        }
    }
}
