using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace EFWhatToRead_DAL.Repositories.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllEntitiesAsync();
        Task<TEntity> GetEntityByIdAsync(int id);
        Task<TEntity> CreateEntityAsync(TEntity entity);
        Task<bool> UpdateEntityAsync(TEntity entity);
        Task<bool> DeleteEntityAsync(int id);
    }
}
