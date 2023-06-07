using Application.Interfaces.Repositories;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Persistence.Repositories
{
    public class GenericRepositoryMongo<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepositoryMongo(IMongoDatabase database)
        {
            var collectionName = typeof(T).Name + "s";
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await _collection.ReplaceOneAsync(e => e._id == entity._id, entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await _collection.DeleteOneAsync(e => e._id == entity._id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(e => e._id == id).FirstOrDefaultAsync();
        }
    }
}
