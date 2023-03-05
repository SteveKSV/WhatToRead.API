﻿using EFTopics.DAL.Entities;
using EFTopics.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTopics.DAL.Data.Repositories
{
    public class TopicsRepository : GenericRepository<Topic>, ITopicsRepository
    {
        public TopicsRepository(ApplicationContext _dbcontext) : base(_dbcontext) { }
    }
}
