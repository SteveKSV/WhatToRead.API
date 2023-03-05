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
    public class TopicsSeeder : ISeeder<Topic>
    {
        private static readonly List<Topic> topics = new()
        {
            new Topic
            {
                TopicId = 1,
                Name = "Books"
            }
        };

        public void Seed(EntityTypeBuilder<Topic> builder) => builder.HasData(topics);
    }
}
