using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodRepository
    {
        public List<Food> GetFoodsByFullTextSearch(string query);
    }
}
