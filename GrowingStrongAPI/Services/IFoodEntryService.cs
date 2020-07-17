using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;

namespace GrowingStrongAPI.Services
{
    public interface IFoodEntryService
    {
        public void UpdateFoodEntry(int foodEntryId, FoodEntryUpdateModel updateModel);
    }
}
