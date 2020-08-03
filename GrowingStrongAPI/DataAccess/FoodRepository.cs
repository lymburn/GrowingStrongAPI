using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;
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

        public void CreateFood(FoodDto foodDto)
        {
            //Create food first
            int foodId = foodDto.FoodId;
            string foodName = foodDto.FoodName;

            string createFoodSql = $"call create_food({foodId}, '{foodName}')";

            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                connection.Open();

                connection.Execute(createFoodSql);
            }

            //Create servings of food
            List<ServingDto> servings = foodDto.Servings.AsList();

            foreach (ServingDto serving in servings)
            {
                double quantity = serving.Quantity;
                string unit = serving.Unit;
                double kcal = serving.Kcal;
                double carb = serving.Carb;
                double fat = serving.Fat;
                double protein = serving.Protein;

                string createServingSql = $"call create_serving({foodId}, {quantity}, '{unit}', {kcal}, {carb}, {fat}, {protein})";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(createServingSql);
                }
            }
        }
    }
}
