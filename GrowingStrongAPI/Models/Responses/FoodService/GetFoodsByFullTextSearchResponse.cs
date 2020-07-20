using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Models
{
    public class GetFoodsByFullTextSearchResponse : BaseResponse
    {
        public IList<FoodDto> FoodDtos;

        public GetFoodsByFullTextSearchResponse()
        {
            FoodDtos = new List<FoodDto>();
        }
    }
}
