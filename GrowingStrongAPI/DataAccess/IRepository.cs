using System;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.DataAccess
{
    public interface IRepository<T> where T : EntityBase
    {
        T GetById(int id);
        void Create(T entity);
    }
}
