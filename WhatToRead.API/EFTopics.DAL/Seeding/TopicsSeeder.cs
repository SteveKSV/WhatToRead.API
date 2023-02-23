using EFTopics.DAL.Builders;
using EFTopics.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.DAL.Seeding
{
    public class TopicsSeeder : ISeeder<Topics>
    {
        private static readonly List<Topics> topics = new()
        {
            new Topics
            {
                Id = 1,
                Name = "Books"
            }
        };

        public void Seed(EntityTypeBuilder<Topics> builder) => builder.HasData(topics);
    }
}
