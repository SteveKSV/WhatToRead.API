using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToRead.API.AdoNet.BBL.Managers.Interfaces
{
    public interface IGenericManager<T> where T : class
    {
        Task<T> Create(T entity);
        Task<IEnumerable<T>> GetAllEntities();
        Task<T> GetEntityById(int id);
        Task UpdateEntityById(T entity);
        Task DeleteEntityById(int id);
    }
}
