using EFTopics.DAL.Dtos;
using EFWhatToRead_BBL.Dtos;
using EFWhatToRead_DAL.Params;

namespace EFWhatToRead_BBL.Managers.Interfaces
{
    public interface IPostManager
    {
        Task<PostDto> CreatePost(PostDto topic);
        Task<IEnumerable<PostDto>> GetAllPosts(PageModel pagination);
        Task<PostDto> GetPostById(int postId);
        Task<bool> UpdatePostById(PostDto entity);
        Task<bool> DeletePostById(int postId);
        Task<List<PostWithTopicsDto>> GetAllPostsWithTopics();
        Task<PostWithTopicsDto?> GetPostByIdWithTopics(int postId);
    }
}
