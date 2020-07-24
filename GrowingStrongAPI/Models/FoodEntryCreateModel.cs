using System;
namespace GrowingStrongAPI.Models
{
    public class FoodEntryCreateModel
    {
        public Guid FoodEntryId { get; set; }

        public int UserId { get; set; }

        public int FoodId { get; set; }

        public DateTime DateAdded { get; set; }

        public double ServingAmount { get; set; }

        public int SelectedServingId { get; set; }
    }
}
