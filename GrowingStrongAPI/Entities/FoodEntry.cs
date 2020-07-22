using System;
namespace GrowingStrongAPI.Entities
{
    public class FoodEntry
    {
        public Guid FoodEntryId { get; set; }
        public User User { get; set; }
        public Food Food { get; set; }
        public DateTime DateAdded { get; set; }
        public double ServingAmount { get; set; }
        public Serving SelectedServing { get; set; }
    }
}
