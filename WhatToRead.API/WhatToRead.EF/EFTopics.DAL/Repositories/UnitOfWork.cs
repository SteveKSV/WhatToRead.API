﻿using EFTopics.BBL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFTopics.BBL.Data;
using EFWhatToRead_DAL.Repositories.Interfaces.Repositories;
using EFWhatToRead_DAL.Repositories.Interfaces;

namespace EFWhatToRead_DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationContext databaseContext;

        public ITopicsRepository TopicsRepository { get; }
        public IPostRepository PostRepository { get; }
        public async Task SaveChangesAsync()
        {
            await databaseContext.SaveChangesAsync();
        }

        public UnitOfWork(
            ApplicationContext databaseContext,
            ITopicsRepository topicsRepository,
            IPostRepository postRepository)
        {
            this.databaseContext = databaseContext;

            TopicsRepository = topicsRepository;
            PostRepository = postRepository;
        }
    }
}
