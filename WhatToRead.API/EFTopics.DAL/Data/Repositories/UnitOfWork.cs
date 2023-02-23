using EFTopics.DAL.Entities;
using EFTopics.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamworkSystem.DataAccessLayer.Interfaces;

namespace EFTopics.DAL.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly TopicsContext databaseContext;

        public ITopicsRepository TopicsRepository { get; }
        public async Task SaveChangesAsync()
        {
            await databaseContext.SaveChangesAsync();
        }

        public UnitOfWork(
            TopicsContext databaseContext,
            ITopicsRepository topicsRepository)
        {
            this.databaseContext = databaseContext;

            TopicsRepository = topicsRepository;
        }
    }
}
