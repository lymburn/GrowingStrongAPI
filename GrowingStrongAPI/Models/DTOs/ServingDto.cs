using System;
using System.ComponentModel.DataAnnotations;

namespace GrowingStrongAPI.Models
{
    public class ServingDto
    {
        [Required]
        public int ServingId { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public string Unit { get; set; }

        [Required]
        public double Kcal { get; set; }

        public double Carb { get; set; }

        public double Fat { get; set; }

        public double Protein { get; set; }

    }
}
