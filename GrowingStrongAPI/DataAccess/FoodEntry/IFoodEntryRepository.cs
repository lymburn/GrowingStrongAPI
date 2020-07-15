using System;
using GrowingStrongAPI.Entities;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodEntryRepository
    {
        FoodEntry GetByUserAndFoodId(int userId, int foodId);
        List<FoodEntry> GetFoodEntriesOfUser(int userId);
    }
}
