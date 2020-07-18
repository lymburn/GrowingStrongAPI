using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IFoodEntryService
    {
        public CreateFoodEntryResponse CreateFoodEntry(FoodEntryCreateModel createModel);
        public UpdateFoodEntryResponse UpdateFoodEntry(int foodEntryId, FoodEntryUpdateModel updateModel);
        public DeleteFoodEntryResponse DeleteFoodEntry(int foodEntryId);
    }
}
