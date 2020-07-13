using System;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using Dapper;
using Npgsql;
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
                      ILogger<IUserRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public FoodEntry GetByUserAndFoodId(int userId, int foodId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FoodEntry> GetFoodEntriesOfUser(int userId)
        {
            string sql = $"select * from get_user_food_entries ({userId})";

            using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
            {
                connection.Open();

                var foodEntries = connection.Query<FoodEntry, Food, Serving, FoodEntry>(
                    sql,
                    map: (fe, f, s) =>
                    {
                        fe.Food = f;
                        fe.SelectedServing = s;
                        s.Food = f;

                        return fe;
                    },
                    splitOn: "food_id, serving_id");

                return foodEntries;
            }

        }
    }
}
