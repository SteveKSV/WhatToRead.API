using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Seeding;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.BBL.Seeding
{
    public class TopicsSeeder : ISeeder<Topic>
    {
        private readonly List<Topic> topics = new()
        {
            new Topic
            {
                TopicId = 1,
                Name = "Books"
            },
            new Topic
            {
                TopicId = 2,
                Name = "Movie"
            }

        };

        public void Seed(EntityTypeBuilder<Topic> builder) => builder.HasData(topics);
    }
}
