using AutoMapper;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_DAL.Params;
using EFWhatToRead_DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Managers
{
    public class PostManager : IPostManager
    {
        public PostManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }
        public async Task<PostDto> CreatePost(PostDto post)
        {
            var entity = Mapper.Map<PostDto, Post>(post);
            var response = await UnitOfWork.PostRepository.CreateEntityAsync(entity);

            return Mapper.Map<PostDto>(response);
        }

        public async Task<bool> DeletePostById(int postId)
        {
            var response = await UnitOfWork.PostRepository.DeleteEntityAsync(postId);
            return response;
        }

        public async Task<IEnumerable<PostDto>> GetAllPosts(PageModel pagination)
        {
            var response = await UnitOfWork.PostRepository.GetAllEntitiesAsync(pagination.PageNumber, pagination.PageSize);

            return Mapper.Map<IEnumerable<PostDto>>(response);
        }

        public async Task<PostDto> GetPostById(int postId)
        {
            var response = await UnitOfWork.PostRepository.GetEntityByIdAsync(postId);
            return Mapper.Map<PostDto>(response);
        }

        public async Task<bool> UpdatePostById(PostDto entity)
        {
            var response = await UnitOfWork.PostRepository.UpdateEntityAsync(Mapper.Map<Post>(entity));
            return response;
        }
    }
}
