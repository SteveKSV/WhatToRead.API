using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTopics.DAL.Entities;
using EFTopics.DAL.Seeding;

namespace EFTopics.DAL.Configuration
{
    public class TopicsConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder
                .HasMany(t => t.PostBlogs)
                .WithOne(t => t.Topic)
                .HasForeignKey(t => t.TopicId)
                .HasPrincipalKey(t => t.TopicId);

            builder.Property(project => project.TopicId)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(project => project.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            new TopicsSeeder().Seed(builder);
        }
    }
}