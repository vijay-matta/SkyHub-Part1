using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Flight_Details
{
    public class BaggageInfos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1) in the database
        public int Id { get; set; }

        [Required]
        [Range(0, 50, ErrorMessage = "Check-in weight must be between 0 and 50 kg.")]
        public decimal CheckinWeight { get; set; }

        [Required]
        [Range(0, 15, ErrorMessage = "Cabin weight must be between 0 and 15 kg.")]
        public decimal CabinWeight { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Excess baggage rate must be a positive value.")]
        [Column(TypeName = "decimal(10, 2)")] // Precision and scale for database
        public decimal ExcessBaggageRate { get; set; }

        // Navigation property
        public virtual Flights Flight { get; set; }
        public int FlightId { get; set; }  // Foreign key to Flights table
    }
}
