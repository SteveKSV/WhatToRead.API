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
    public class AuthorManager : IAuthorManager
    {
        public AuthorManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }
        public async Task<AuthorDTO> Create(AuthorDTO entityDTO)
        {
            var entity = Mapper.Map<AuthorDTO, Author>(entityDTO);
            var response = await UnitOfWork.Authors.AddAsync(entity);
            UnitOfWork.Commit();
            return Mapper.Map<AuthorDTO>(entity);
        }

        public async Task DeleteEntityById(int id)
        {
            await UnitOfWork.Authors.DeleteAsync(id);
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllEntities()
        {
            var response = await UnitOfWork.Authors.GetAllAsync();
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<AuthorDTO>>(response);
        }

        public async Task<AuthorDTO> GetEntityById(int id)
        {
            var response = await UnitOfWork.Authors.GetByIdAsync(id);
            UnitOfWork.Commit();
            return Mapper.Map<AuthorDTO>(response);
        }

        public async Task UpdateEntityById(AuthorDTO entity)
        {
            await UnitOfWork.Authors.UpdateAsync(Mapper.Map<AuthorDTO, Author>(entity));
        }
    }
}
