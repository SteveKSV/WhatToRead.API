using EFTopics.DAL.Entities;
using EFTopics.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamworkSystem.DataAccessLayer.Data.Repositories;

namespace EFTopics.DAL.Data.Repositories
{
    public class TopicsRepository : GenericRepository<Topics>, ITopicsRepository
    {
        public TopicsRepository(TopicsContext topicsContext) : base(topicsContext) { }

        public override async Task UpdateAsync(int id, Topics entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Topics)} entity must not be null");
            }
            var topicsToUpdate = databaseContext.Topics.Find(id);
            topicsToUpdate.Name = entity.Name;
            await Task.Run(()=> table.Update(topicsToUpdate));
        }
    }
}
