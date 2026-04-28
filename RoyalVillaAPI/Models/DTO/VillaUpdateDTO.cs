using System.ComponentModel.DataAnnotations;

namespace RoyalVillaAPI.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; } // có thể nhận giá trị null
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; } // có thể nhận giá trị null
        
    }
}
