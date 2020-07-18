using System;
namespace GrowingStrongAPI.Models
{
    public class FoodEntryCreateModel
    {
        public int UserId { get; set; }

        public int FoodId { get; set; }

        public string DateAdded { get; set; }

        public double ServingAmount { get; set; }

        public int SelectedServingId { get; set; }
    }
}
