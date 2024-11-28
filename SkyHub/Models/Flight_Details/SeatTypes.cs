using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Flight_Details
{
    public class SeatTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int SeatTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 150, ErrorMessage = "Age must be a valid number.")]
        public int Age { get; set; }

        [Required]
        [RegularExpression("Adult|Child|Infant", ErrorMessage = "Passenger type must be either Adult, Child, or Infant.")]
        public string PassengerType { get; set; } // Adult, Child, Infant

        [Required, StringLength(50)]
        public string SeatTypeName { get; set; } // e.g., Economy, Business, First Class

        [Required]
        public decimal BaseFare { get; set; }

        // Navigation properties
        public ICollection<Seats> Seats { get; set; } // Many Seats can have the same SeatType
        public ICollection<BookingItems> BookingItems { get; set; } // Many BookingItems can have the same SeatType
    }
}
