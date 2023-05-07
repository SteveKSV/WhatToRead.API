using EFTopics.DAL.Entities;

namespace EFWhatToRead_DAL.Entities
{
    public class TopicsWithPost
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
