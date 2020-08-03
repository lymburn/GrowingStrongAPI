using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodRepository
    {
        public List<Food> GetFoodsByFullTextSearch(string query);
        public void CreateFood(FoodDto foodDto);
    }
}
