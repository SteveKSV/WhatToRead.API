using System.Collections.Generic;
using System.Threading.Tasks;
using EFTopics.DAL.Data;
using Microsoft.EntityFrameworkCore;
using TeamworkSystem.DataAccessLayer.Exceptions;
using TeamworkSystem.DataAccessLayer.Interfaces.Repositories;

namespace TeamworkSystem.DataAccessLayer.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly TopicsContext databaseContext;

        protected readonly DbSet<TEntity> table;

        public async Task<IEnumerable<TEntity>> GetAllTopicsAsync() => await table.ToListAsync();

        public async Task<TEntity> GetTopicByIdAsync(int id)
        {
            return await table.FindAsync(id)
                ?? throw new EntityNotFoundException(
                    GetEntityNotFoundErrorMessage(id));
        }

        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
            }
            await table.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(int id, TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(TEntity)} entity must not be null");
            }
            await Task.Run(() => table.Update(entity));
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetTopicByIdAsync(id);
            await Task.Run(() => table.Remove(entity));
        }

        protected static string GetEntityNotFoundErrorMessage(int id) =>
            $"{typeof(TEntity).Name} with id {id} not found.";

        public GenericRepository(TopicsContext databaseContext)
        {
            this.databaseContext = databaseContext;
            table = this.databaseContext.Set<TEntity>();
        }
    }
}
