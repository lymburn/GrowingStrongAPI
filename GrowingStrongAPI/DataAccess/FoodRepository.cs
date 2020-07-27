using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using Dapper;
using System.Linq;
using GrowingStrongAPI.Helpers;

namespace GrowingStrongAPI.DataAccess
{
    public class FoodRepository : IFoodRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public FoodRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public List<Food> GetFoodsByFullTextSearch(string query)
        {
            try
            {
                string sql = $"select * from get_foods_by_full_text_search('{query}')";

                var foodDictionary = new Dictionary<int, Food>();

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    List<Food> foods = connection.Query<Food, Serving, Food>(
                        sql,
                        map: (food, serving) =>
                        {
                            Food mappedFood;

                            if (!foodDictionary.TryGetValue(food.FoodId, out mappedFood))
                            {
                                mappedFood = food;
                                mappedFood.Servings = new List<Serving>();

                                foodDictionary.Add(mappedFood.FoodId, mappedFood);
                            }

                            mappedFood.Servings.Add(serving);

                            return mappedFood;
                        },
                        splitOn: "serving_id").Distinct().ToList();

                    return foods;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
