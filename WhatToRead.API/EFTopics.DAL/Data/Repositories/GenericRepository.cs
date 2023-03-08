using System.Collections.Generic;
using System.Threading.Tasks;
using EFTopics.DAL.Data;
using EFTopics.DAL.Exceptions;
using EFTopics.DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace EFTopics.DAL.Data.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _dbContext;
        private readonly DbSet<TEntity> table;
        public GenericRepository(ApplicationContext databaseContext)
        {
            _dbContext = databaseContext;
            table = _dbContext.Set<TEntity>();
        }

        public async Task<bool> CreateEntityAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteEntityAsync(TEntity entity)
        {
            _dbContext.Remove(entity);
            return await SaveAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<TEntity> GetEntityByIdAsync(int id)
        {
            var entity = await table.FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(
                GetEntityNotFoundErrorMessage(id));
            }
            
            return entity;
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateEntityAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            return await SaveAsync();
        }

        protected static string GetEntityNotFoundErrorMessage(int id) =>
            $"{typeof(TEntity).Name} with id {id} not found.";
    }
}
