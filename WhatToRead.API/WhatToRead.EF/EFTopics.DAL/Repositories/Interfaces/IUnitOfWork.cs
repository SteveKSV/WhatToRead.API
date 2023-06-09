using System.Threading.Tasks;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;

namespace EFWhatToRead_DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITopicsRepository TopicsRepository { get; }
        IPostRepository PostRepository { get; }
        Task SaveChangesAsync();
    }
}
