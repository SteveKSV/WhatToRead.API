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
    public class PublisherManager : IPublisherManager
    {
        public PublisherManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        private IUnitOfWork UnitOfWork { get; }
        private IMapper Mapper { get; }
        public async Task<PublisherDTO> Create(PublisherDTO entityDTO)
        {
            var entity = Mapper.Map<PublisherDTO, Publisher>(entityDTO);
            var response = await UnitOfWork.Publishers.AddAsync(entity);
            UnitOfWork.Commit();
            return Mapper.Map<PublisherDTO>(entity);
        }

        public async Task DeleteEntityById(int id)
        {
            await UnitOfWork.Publishers.DeleteAsync(id);
        }

        public async Task<IEnumerable<PublisherDTO>> GetAllEntities()
        {
            var response = await UnitOfWork.Publishers.GetAllAsync();
            UnitOfWork.Commit();

            return Mapper.Map<IEnumerable<PublisherDTO>>(response);
        }

        public async Task<PublisherDTO> GetEntityById(int id)
        {
            var response = await UnitOfWork.Publishers.GetByIdAsync(id);
            UnitOfWork.Commit();
            return Mapper.Map<PublisherDTO>(response);
        }

        public async Task UpdateEntityById(PublisherDTO entity)
        {
            await UnitOfWork.Publishers.UpdateAsync(Mapper.Map<Publisher>(entity));
        }
    }
}
