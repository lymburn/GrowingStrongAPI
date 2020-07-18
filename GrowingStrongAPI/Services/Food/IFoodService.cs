using System;
using GrowingStrongAPI.Models;

namespace GrowingStrongAPI.Services
{
    public interface IFoodService
    {
        public GetFoodsStartingWithPatternResponse GetFoodsStartingWithPattern(string pattern);
    }
}
