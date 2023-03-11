using AutoMapper;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var entity = Mapper.Map<Topic>(topic);
            var response = await UnitOfWork.TopicsRepository.CreateEntityAsync(entity);
            
            return Mapper.Map<TopicDto>(response);
        }

        public async Task<bool> DeleteTopicById(int topicId)
        {
            var response = await UnitOfWork.TopicsRepository.DeleteEntityAsync(topicId);
            return response;
        }

        public async Task<IEnumerable<TopicDto>> GetAllTopics()
        {
            var response = await UnitOfWork.TopicsRepository.GetAllEntitiesAsync();

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
    }
}
