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
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasMany(p => p.PostBlogs)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId)
                .HasPrincipalKey(p => p.PostId);

            builder.Property(p => p.PostId)
                   .UseIdentityColumn()
                   .IsRequired();
        }
    }
}
