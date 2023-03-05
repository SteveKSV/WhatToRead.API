using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.DB.Models.DTO;
using WhatToRead.Core.Models;

namespace WhatToRead.API.AdoNet.DB.Repositories.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<BookByAuthor>> GetBookByAuthorId(int id);
        Task<IEnumerable<BookByPublisher>> GetBookByPublisherId(int id);
        Task<IEnumerable<BooksWithPublisher>> GetAllBooksWithPublisherName();

        Task<IEnumerable<Book>> GetBooksByDateUp(DateTime date);
    }
}
