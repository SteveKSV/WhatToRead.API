using EFTopics.BBL.Entities;
using EFWhatToRead_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Repositories.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<PostWithTopics>> GetAllPostsWithTopics();
        Task<PostWithTopics?> GetPostByIdWithTopics(int postId);
    }
}
