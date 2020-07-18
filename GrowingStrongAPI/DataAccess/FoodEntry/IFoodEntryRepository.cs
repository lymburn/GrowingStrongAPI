using System;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodEntryRepository
    {
        FoodEntry GetByUserAndFoodId(int userId, int foodId);
        List<FoodEntry> GetFoodEntriesOfUser(int userId);
        void CreateFoodEntry(FoodEntryCreateModel createModel);
        void UpdateFoodEntry(int foodEntryId, FoodEntryUpdateModel updateModel);
        void DeleteFoodEntry(int foodEntryId);
    }
}
