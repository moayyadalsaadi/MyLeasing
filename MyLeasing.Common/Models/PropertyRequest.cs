using System.ComponentModel.DataAnnotations;

namespace MyLeasing.Common.Models
{
    public class PropertyRequest
    {
        public int Id { get; set; }

        [Required]
        public string Neighborhood { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int SquareMeters { get; set; }
       
        public int? Rooms { get; set; }
       
        public int? Bathrooms { get; set; }
    
        public int? Balconies { get; set; }

        public int? Stratum { get; set; }

        [Required]
        public string Typeprop { get; set; }

        public bool HasParkingLot { get; set; }

        public bool IsAvailable { get; set; }

        public string Remarks { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
