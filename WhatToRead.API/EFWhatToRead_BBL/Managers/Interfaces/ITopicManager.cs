using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_DAL.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Managers.Interfaces
{
    public interface ITopicManager
    {
        Task<TopicDto> CreateTopic (TopicDto topic);
        Task<IEnumerable<TopicDto>> GetAllTopics (PageModel pagination);
        Task<TopicDto> GetTopicById(int topicId);
        Task<bool> UpdateTopicById(TopicDto entity);
        Task<bool> DeleteTopicById(int topicId);

        Task<List<TopicsWithPostsDto>> GetAllTopicsWithPosts();
        Task<TopicsWithPostsDto?> GetTopicByIdWithPosts(int topicId);
    }
}
