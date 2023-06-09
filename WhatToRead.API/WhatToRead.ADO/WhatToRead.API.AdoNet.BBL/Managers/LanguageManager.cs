using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.BBL.Managers.Interfaces;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;

namespace WhatToRead.API.AdoNet.BBL.Managers
{
    public class LanguageManager : ILanguageManager
    {
        public LanguageManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }
        public async Task<Book_LanguageDTO> Create(Book_LanguageDTO entityDTO)
        {
            var entity = Mapper.Map<Book_LanguageDTO, Book_Language>(entityDTO);
            var response = await UnitOfWork.Languages.AddAsync(entity);
            UnitOfWork.Commit();
            return Mapper.Map<Book_LanguageDTO>(entity);
        }

        public async Task DeleteEntityById(int id)
        {
            await UnitOfWork.Languages.DeleteAsync(id);
        }

        public async Task<IEnumerable<Book_LanguageDTO>> GetAllEntities()
        {
            var response = await UnitOfWork.Languages.GetAllAsync();
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<Book_LanguageDTO>>(response);
        }

        public async Task<Book_LanguageDTO> GetEntityById(int id)
        {
            var response = await UnitOfWork.Languages.GetByIdAsync(id);
            UnitOfWork.Commit();
            return Mapper.Map<Book_LanguageDTO>(response);
        }

        public async Task UpdateEntityById(Book_LanguageDTO entity)
        {
            await UnitOfWork.Languages.UpdateAsync(Mapper.Map<Book_Language>(entity));
        }
    }
}
