using System;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.DataAccess
{
    public class Repository <T> : IRepository<T> where T : EntityBase
    {
        public Repository()
        {
        }
    }
}
