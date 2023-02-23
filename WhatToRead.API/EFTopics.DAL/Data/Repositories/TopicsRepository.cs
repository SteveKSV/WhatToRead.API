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

        public override Task<Topics> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
