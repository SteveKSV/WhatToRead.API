using AutoMapper;
using EFTopics.BBL.Entities;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_DAL.Params;
using EFWhatToRead_DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFWhatToRead_BBL.Managers
{
    public class TopicManager : ITopicManager
    {
        public TopicManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }

        public async Task<TopicDto> CreateTopic(TopicDto topic)
        {
            var entity = Mapper.Map<TopicDto, Topic>(topic);
            var response = await UnitOfWork.TopicsRepository.CreateEntityAsync(entity);
            
            return Mapper.Map<TopicDto>(response);
        }

        public async Task<bool> DeleteTopicById(int topicId)
        {
            var response = await UnitOfWork.TopicsRepository.DeleteEntityAsync(topicId);
            return response;
        }

        public async Task<IEnumerable<TopicDto>> GetAllTopics(PageModel pagination)
        {
            var response = await UnitOfWork.TopicsRepository.GetAllEntitiesAsync(pagination.PageNumber, pagination.PageSize);

            return Mapper.Map<IEnumerable<TopicDto>>(response);
        }

        public async Task<TopicDto> GetTopicById(int topicId)
        {
            var response = await UnitOfWork.TopicsRepository.GetEntityByIdAsync(topicId);
            return Mapper.Map<TopicDto>(response);
        }

        public async Task<bool> UpdateTopicById(TopicDto topicDto)
        {
            var response = await UnitOfWork.TopicsRepository.UpdateEntityAsync(Mapper.Map<Topic>(topicDto));
            return response;
        }

        public async Task<List<TopicsWithPostsDto>> GetAllTopicsWithPosts()
        {
            var entities = await UnitOfWork.TopicsRepository.GetAllTopicsWithPosts();
            return Mapper.Map<List<TopicsWithPostsDto>>(entities);
        }

        public async Task<TopicsWithPostsDto?> GetTopicByIdWithPosts(int topicId)
        {
            var topic = await UnitOfWork.TopicsRepository.GetTopicByIdWithPosts(topicId);
            return Mapper.Map<TopicsWithPostsDto>(topic);
        }

        public async Task<int> GetTotalItems()
        {
            try
            {
                int count = await UnitOfWork.TopicsRepository.GetTotalItems();
                return count;
            }
            catch (Exception ex)
            {
                // Handle any exceptions or logging as needed
                throw new Exception("Failed to retrieve total item count.", ex);
            }
        }
    }
}
