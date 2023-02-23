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
    public class TopicsConfiguration : IEntityTypeConfiguration<Topics>
    {
        public void Configure(EntityTypeBuilder<Topics> builder)
        {
            builder.Property(project => project.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(project => project.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            new TopicsSeeder().Seed(builder);
        }
    }
}