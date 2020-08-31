using System;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Services
{
    public interface IFoodService
    {
        public GetFoodsByFullTextSearchResponse GetFoodsByFullTextSearch(string query);
        public CreateFoodResponse CreateFood(FoodDto foodDto);
    }
}
