using System;
using GrowingStrongAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GrowingStrongAPI.Models
{
    public class FoodEntryDto
    {
        [Required]
        public int FoodEntryId { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public double ServingAmount { get; set; }

        [Required]
        public FoodDto Food { get; set; }

        [Required]
        public ServingDto SelectedServing { get; set; }
    }   
}