using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamworkSystem.DataAccessLayer.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllTopicsAsync();

        Task<TEntity> GetTopicByIdAsync(int id);

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(int id, TEntity entity);

        Task DeleteAsync(int id);
    }
}
