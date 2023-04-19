using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.DB.Models.DTO;
using WhatToRead.Core.Models;

namespace WhatToRead.API.AdoNet.BBL.Managers.Interfaces
{
    public interface IBookManager : IGenericManager<BookDTO>
    {
        Task<IEnumerable<BookByAuthor>> GetBookByAuthorId(int id);
        Task<IEnumerable<BookByPublisher>> GetBookByPublisherId(int id);
        Task<IEnumerable<BooksWithPublisher>> GetAllBooksWithPublisherName();
        Task<IEnumerable<Book>> GetBooksByDateUp(DateTime date);

    }
}
