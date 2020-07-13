using System;
using GrowingStrongAPI.Entities;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodEntryRepository
    {
        FoodEntry GetByUserAndFoodId(int userId, int foodId);
        IEnumerable<FoodEntry> GetFoodEntriesOfUser(int userId);
    }
}
