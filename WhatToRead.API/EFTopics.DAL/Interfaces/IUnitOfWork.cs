using System.Threading.Tasks;
using EFTopics.DAL.Interfaces.Repositories;

namespace EFTopics.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ITopicsRepository TopicsRepository { get; }
        IPostRepository PostRepository { get; }
        Task SaveChangesAsync();
    }
}
