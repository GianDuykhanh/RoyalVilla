using System.ComponentModel.DataAnnotations;

namespace RoyalVillaAPI.Models.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Details { get; set; } // có thể nhận giá trị null
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; } // có thể nhận giá trị null        
    }
}
