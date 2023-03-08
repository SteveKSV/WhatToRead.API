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
                .HasMany(p => p.Topics)
                .WithMany(p => p.Posts)
                .UsingEntity<PostBlog>(
                    j => j
                        .HasOne(pt => pt.Topic)
                        .WithMany(t => t.PostBlogs)
                        .HasForeignKey(t => t.TopicId)
                        .HasPrincipalKey(t=>t.TopicId),
                    j => j
                        .HasOne(pt => pt.Post)
                        .WithMany(pt => pt.PostBlogs)
                        .HasForeignKey(t => t.PostId)
                        .HasPrincipalKey(t => t.PostId),
                    j =>
                    {
                        j.HasKey(t=> new {t.PostId, t.TopicId});
                        j.ToTable("PostBlogs");
                    });
            new PostSeeder().Seed(builder);
        }
    }
}
