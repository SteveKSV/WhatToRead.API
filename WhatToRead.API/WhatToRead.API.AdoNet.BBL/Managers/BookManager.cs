using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.BBL.Managers.Interfaces;
using WhatToRead.API.AdoNet.DB.Models.DTO;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;

namespace WhatToRead.API.AdoNet.BBL.Managers
{
    public class BookManager : IBookManager
    {
        public BookManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }
        public async Task<BookDTO> Create(BookDTO entityDTO)
        {
            var entity = Mapper.Map<BookDTO, Book>(entityDTO);
            var response = await UnitOfWork.Books.AddAsync(entity);
            UnitOfWork.Commit();
            return Mapper.Map<BookDTO>(entity);
        }

        public async Task DeleteEntityById(int id)
        {
            await UnitOfWork.Books.DeleteAsync(id);
        }

        public async Task<IEnumerable<BooksWithPublisher>> GetAllBooksWithPublisherName()
        {
            var response = await UnitOfWork.Books.GetAllBooksWithPublisherName();
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<BooksWithPublisher>>(response);
        }

        public async Task<IEnumerable<BookDTO>> GetAllEntities()
        {
            var response = await UnitOfWork.Books.GetAllAsync();
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<BookDTO>>(response);
        }

        public async Task<IEnumerable<BookByAuthor>> GetBookByAuthorId(int id)
        {
            var response = await UnitOfWork.Books.GetBookByAuthorId(id);
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<BookByAuthor>>(response);
        }

        public async Task<IEnumerable<BookByPublisher>> GetBookByPublisherId(int id)
        {
            var response = await UnitOfWork.Books.GetBookByPublisherId(id);
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<BookByPublisher>>(response);
        }

        public async Task<IEnumerable<Book>> GetBooksByDateUp(DateTime date)
        {
            var response = await UnitOfWork.Books.GetBooksByDateUp(date);
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<Book>>(response);
        }

        public async Task<BookDTO> GetEntityById(int id)
        {
            var response = await UnitOfWork.Books.GetByIdAsync(id);
            UnitOfWork.Commit();
            return Mapper.Map<BookDTO>(response);
        }

        public async Task UpdateEntityById(BookDTO entity)
        {
            await UnitOfWork.Books.UpdateAsync(Mapper.Map<BookDTO, Book>(entity));
        }
    }
}
