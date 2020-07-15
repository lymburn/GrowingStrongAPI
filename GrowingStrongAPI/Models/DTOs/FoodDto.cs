using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowingStrongAPI.Models
{
    public class FoodDto
    {
        [Required]
        public int FoodId { get; set; }

        [Required]
        public string FoodName { get; set; }

        [Required]
        public IList<ServingDto> Servings { get; set; }
    }
}
