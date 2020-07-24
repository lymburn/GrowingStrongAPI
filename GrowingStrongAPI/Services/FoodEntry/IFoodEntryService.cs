using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Services
{
    public interface IFoodEntryService
    {
        public CreateFoodEntryResponse CreateFoodEntry(FoodEntryCreateModel createModel);
        public UpdateFoodEntryResponse UpdateFoodEntry(Guid foodEntryId, FoodEntryUpdateModel updateModel);
        public DeleteFoodEntryResponse DeleteFoodEntry(Guid foodEntryId);
    }
}
