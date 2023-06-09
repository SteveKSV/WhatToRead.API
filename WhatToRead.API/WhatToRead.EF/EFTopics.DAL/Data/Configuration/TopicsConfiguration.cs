using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTopics.BBL.Entities;
using EFTopics.BBL.Seeding;

namespace EFWhatToRead_DAL.Data.Configuration
{
    public class TopicsConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {

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