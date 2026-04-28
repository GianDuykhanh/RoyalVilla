using System.ComponentModel.DataAnnotations;

namespace RoyalVillaAPI.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Details { get; set; } // có thể nhận giá trị null
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; } // có thể nhận giá trị null
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
