using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Models
{
    public class GetFoodsStartingWithPatternResponse : BaseResponse
    {
        public IList<FoodDto> FoodDtos;

        public GetFoodsStartingWithPatternResponse()
        {
            FoodDtos = new List<FoodDto>();
        }
    }
}
