using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Flight_Details
{
    public class BookingItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int BookingItemId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int SeatId { get; set; }

        [Required]
        public int SeatTypeId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0.")]
        public decimal Price { get; set; }

        // Navigation Properties
        public virtual Bookings Booking { get; set; }
        public virtual Seats Seat { get; set; }
        public virtual SeatTypes SeatType { get; set; }
    }
}
