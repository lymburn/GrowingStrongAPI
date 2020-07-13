using System;
using System.Collections.Generic;
namespace GrowingStrongAPI.Entities
{
    public class Food
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public IList<Serving> Servings { get; set; }
    }
}
