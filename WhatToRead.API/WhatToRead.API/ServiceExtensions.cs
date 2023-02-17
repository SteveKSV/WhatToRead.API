using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.Infrastructure.Repositories;

namespace WhatToRead.API
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddTransient<IAuthorRepository, AuthorRepository>();
            service.AddTransient<IBookRepository, BookRepository>();
            service.AddTransient<ILanguageRepository, LanguageRepository>();
            service.AddTransient<IPublisherRepository, PublisherRepository>();
        }
    }
}
