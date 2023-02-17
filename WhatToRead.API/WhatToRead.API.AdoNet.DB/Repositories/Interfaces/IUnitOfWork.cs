using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToRead.API.AdoNet.DB.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        ILanguageRepository Languages { get; }
        IPublisherRepository Publishers { get; }
        void Commit();
        void Dispose();
    }
}
