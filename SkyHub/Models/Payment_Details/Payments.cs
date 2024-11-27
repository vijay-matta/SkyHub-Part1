using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkyHub.Models.Flight_Details;

namespace SkyHub.Models.Payment_Details
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount paid must be greater than or equal to 0.")]
        public decimal AmountPaid { get; set; }

        [Required, StringLength(50)]
        [RegularExpression("Success|Failed|Pending", ErrorMessage = "Invalid Payment Status.")]
        public string PaymentStatus { get; set; }

        [Required, StringLength(50)]
        public string PaymentMode { get; set; }

        [Required, StringLength(100)]
        public string TransactionId { get; set; }

        // Navigation Properties
        public virtual Bookings Booking { get; set; }
        public virtual Refunds Refund { get; set; }
    }
}
