using System.Collections.Generic;
using System.Threading.Tasks;
using EFTopics.DAL.Data;
using EFTopics.DAL.Exceptions;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace EFWhatToRead_DAL.Repositories
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

        public async Task<TEntity> CreateEntityAsync(TEntity entity)
        {
            await table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteEntityAsync(int id)
        {
            var entity = await table.FindAsync(id);
            table.Remove(entity);
            return await SaveAsync();
        }

        public async Task<List<TEntity>> GetAllEntitiesAsync(int pageNumber, int pageSize)
        {
            return await table
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToListAsync();
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

        public async Task<bool> UpdateEntityAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            return await SaveAsync();
        }

        protected static string GetEntityNotFoundErrorMessage(int id) =>
            $"{typeof(TEntity).Name} with id {id} not found.";
                public async Task<bool> SaveAsync()
        {
            var saved = await _dbContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
