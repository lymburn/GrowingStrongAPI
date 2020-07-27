using System;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;
using System.Collections.Generic;

namespace GrowingStrongAPI.DataAccess
{
    public interface IFoodEntryRepository
    {
        List<FoodEntry> GetFoodEntriesOfUser(int userId);
        void CreateFoodEntry(FoodEntryCreateModel createModel);
        void UpdateFoodEntry(Guid foodEntryId, FoodEntryUpdateModel updateModel);
        void DeleteFoodEntry(Guid foodEntryId);
    }
}
