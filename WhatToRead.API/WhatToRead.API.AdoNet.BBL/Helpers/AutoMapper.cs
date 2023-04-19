using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.DB.Models.DTO;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;

namespace WhatToRead.API.AdoNet.BBL.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AuthorDTO, Author>().ReverseMap();
            CreateMap<Book_LanguageDTO, Book_Language>().ReverseMap();
            CreateMap<BookByAuthorDTO, BookByAuthor>().ReverseMap();
            CreateMap<BookByPublisherDTO, BookByPublisher>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
            CreateMap<BookWithPublisherDTO, BooksWithPublisher>().ReverseMap();
            CreateMap<PublisherDTO, Publisher>().ReverseMap();
        }
    }
}
