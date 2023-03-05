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

        public bool CreateEntity(TEntity entity)
        {
            _dbContext.Add(entity);
            return Save();
        }

        public bool DeleteEntity(TEntity entity)
        {
            _dbContext.Remove(entity);
            return Save();
        }

        public ICollection<TEntity> GetAllEntities()
        {
            return table.ToList();
        }

        public TEntity GetEntityById(int id)
        {
            var entity = table.Find(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(
                GetEntityNotFoundErrorMessage(id));
            }
            
            return entity;
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateEntity(TEntity entity)
        {
            _dbContext.Update(entity);
            return Save();
        }

        protected static string GetEntityNotFoundErrorMessage(int id) =>
            $"{typeof(TEntity).Name} with id {id} not found.";
    }
}
