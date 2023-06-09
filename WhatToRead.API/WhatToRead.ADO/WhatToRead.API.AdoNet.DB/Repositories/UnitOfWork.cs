using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;

namespace WhatToRead.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        readonly IDbTransaction _dbTransaction;
        public UnitOfWork
            (IBookRepository bookRepository, IAuthorRepository authorRepository, ILanguageRepository languageRepository,
            IPublisherRepository publisherRepository, IDbTransaction dbTransaction)
        {
            Books = bookRepository;
            Authors = authorRepository;
            Languages = languageRepository;
            Publishers = publisherRepository;
            _dbTransaction = dbTransaction;
        }

        public IBookRepository Books { get; }
        public IAuthorRepository Authors { get; }
        public ILanguageRepository Languages { get; }
        public IPublisherRepository Publishers { get; }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                // By adding this we can have muliple transactions as part of a single request
                //_dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
                Console.WriteLine(ex.Message);
            }
        }
        public void Dispose()
        {
            //Close the SQL Connection and dispose the objects
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
