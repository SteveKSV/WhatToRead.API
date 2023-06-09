using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Entities;

namespace EFWhatToRead_DAL.Repositories.Interfaces.Repositories
{
    public interface ITopicsRepository : IRepository<Topic>
    {
        Task<List<TopicsWithPost>> GetAllTopicsWithPosts();
        Task<TopicsWithPost?> GetTopicByIdWithPosts(int topicId);

        Task<int> GetTotalItems();
    }
}
