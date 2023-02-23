using System.Threading.Tasks;
using EFTopics.DAL.Interfaces.Repositories;
using TeamworkSystem.DataAccessLayer.Interfaces.Repositories;

namespace TeamworkSystem.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork
    {
        ITopicsRepository TopicsRepository { get; }
        Task SaveChangesAsync();
    }
}
