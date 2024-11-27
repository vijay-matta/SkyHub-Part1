using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Flight_Details
{
    public class Seats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int SeatId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required, StringLength(10)]
        public string SeatNumber { get; set; }

        [Required]
        public int SeatTypeId { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public decimal BasePrice { get; set; }

        // Navigation properties
        public Flights Flight { get; set; } // Reference to Flight
        public SeatTypes SeatType { get; set; } // Reference to SeatType
        public ICollection<BookingItems> BookingItems { get; set; } // Many BookingItems for a single Seat

    }
}
