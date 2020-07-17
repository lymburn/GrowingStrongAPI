using System;
using System.Collections.Generic;

namespace GrowingStrongAPI.Models
{
    public class GetUserFoodEntriesResponse: BaseResponse
    {
        public IList<FoodEntryDto> FoodEntryDtos;

        public GetUserFoodEntriesResponse()
        {
            FoodEntryDtos = new List<FoodEntryDto>();
        }
    }
}
