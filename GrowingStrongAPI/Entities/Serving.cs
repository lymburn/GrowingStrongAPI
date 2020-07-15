using System;
namespace GrowingStrongAPI.Entities
{
    public class Serving
    {
        public int ServingId { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Kcal { get; set; }
        public double Carb { get; set; }
        public double Fat { get; set; }
        public double Protein { get; set; }
        public Food Food { get; set; }
    }
}
