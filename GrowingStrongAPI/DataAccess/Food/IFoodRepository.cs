using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodRepository
    {
        public List<Food> GetFoodsStartingWithPattern(string pattern);
    }
}
