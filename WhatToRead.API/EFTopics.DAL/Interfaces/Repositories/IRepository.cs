using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;

namespace EFTopics.DAL.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ICollection<TEntity> GetAllEntities();
        TEntity GetEntityById(int id);
        bool CreateEntity(TEntity entity);
        bool UpdateEntity(TEntity entity);
        bool DeleteEntity(TEntity entity);
        bool Save();
    }
}
