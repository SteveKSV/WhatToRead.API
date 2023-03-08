using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace EFTopics.DAL.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync();
        Task<TEntity> GetEntityByIdAsync(int id);
        Task<bool> CreateEntityAsync(TEntity entity);
        Task<bool> UpdateEntityAsync(TEntity entity);
        Task<bool> DeleteEntityAsync(TEntity entity);
        Task<bool> SaveAsync();
    }
}
