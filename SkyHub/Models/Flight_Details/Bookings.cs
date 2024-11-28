using SkyHub.Models.Payment_Details;
using SkyHub.Models.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Flight_Details
{
    public class Bookings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Number of seats must be at least 1.")]
        public int NumSeats { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int NumAdults { get; set; } // Number of adults (12+ years)

        [Required]
        [Range(0, int.MaxValue)]
        public int NumChildren { get; set; } // Number of children (2-12 years)

        [Required]
        [Range(0, int.MaxValue)]
        public int NumInfants { get; set; } // Number of infants (Below 2 years)

        [Required, StringLength(50)]
        [RegularExpression("Confirmed|Cancelled|Pending", ErrorMessage = "Invalid Booking Status.")]
        public string BookingStatus { get; set; } = "Confirmed";

        public DateTime? CancelDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than or equal to 0.")]
        public decimal TotalPrice { get; set; }


        // Navigation Properties
        public virtual Users User { get; set; }
        public virtual Flights Flight { get; set; }
        public virtual ICollection<BookingItems>? BookingItems { get; set; }
        public virtual Payments Payment { get; set; }
    }
}
