using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Linq;
using GrowingStrongAPI.Helpers;

namespace GrowingStrongAPI.DataAccess
{
    public class FoodEntryRepository : IFoodEntryRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger _logger;

        public FoodEntryRepository(IDbConnectionFactory dbConnectionFactory,
                                   ILogger<IFoodEntryRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public FoodEntry GetByUserAndFoodId(int userId, int foodId)
        {
            throw new NotImplementedException();
        }

        public List<FoodEntry> GetFoodEntriesOfUser(int userId)
        {
            try
            {
                string foodEntriesSql = $"select * from get_user_food_entries ({userId})";

                var foodEntryDictionary = new Dictionary<int, FoodEntry>();

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    List<FoodEntry> foodEntries = connection.Query<FoodEntry, Food, int, Serving, FoodEntry>(
                        foodEntriesSql,
                        map: (foodEntry, food, selected_serving_id, serving) =>
                        {
                            FoodEntry mappedFoodEntry;

                            if (!foodEntryDictionary.TryGetValue(foodEntry.FoodEntryId, out mappedFoodEntry))
                            {
                                mappedFoodEntry = foodEntry;
                                mappedFoodEntry.Food = food;
                                mappedFoodEntry.Food.Servings = new List<Serving>();

                                foodEntryDictionary.Add(mappedFoodEntry.FoodEntryId, mappedFoodEntry);
                            }

                            mappedFoodEntry.Food.Servings.Add(serving);

                            if (serving.ServingId.Equals(selected_serving_id))
                            {
                                foodEntry.SelectedServing = serving;
                            }

                            return mappedFoodEntry;
                        },
                        splitOn: "food_id, selected_serving_id, serving_id").Distinct().ToList();

                    return foodEntries;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateFoodEntry(int foodEntryId, FoodEntryUpdateModel updateModel)
        {
            try
            {
                double servingAmount = updateModel.ServingAmount;

                int selectedServingId = updateModel.SelectedServingId;

                string sql = $"call update_food_entry_serving_size({foodEntryId},{servingAmount},{selectedServingId})";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(sql);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void DeleteFoodEntry(int foodEntryId)
        {
            try
            {
                string sql = $"call delete_food_entry({foodEntryId})";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(sql);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
      
        }
    }
}
