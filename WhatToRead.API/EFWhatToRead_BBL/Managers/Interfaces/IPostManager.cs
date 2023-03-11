using EFTopics.DAL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_BBL.Managers.Interfaces
{
    public interface IPostManager
    {
        Task<PostDto> CreatePost(PostDto topic);
        Task<IEnumerable<PostDto>> GetAllPosts();
        Task<PostDto> GetPostById(int postId);
        Task<bool> UpdatePostById(PostDto entity);
        Task<bool> DeletePostById(int postId);
    }
}
