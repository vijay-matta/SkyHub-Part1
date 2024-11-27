using SkyHub.Models.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkyHub.Models.Flight_Details
{
    public class Flights
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int FlightId { get; set; }

        [Required, StringLength(20)]
        public string FlightNumber { get; set; }

        [Required, StringLength(100)]
        public string FlightName { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ArrivalTime { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DepartureDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ArrivalDate { get; set; }

        // Store return date and time for roundtrip flights
        [Column(TypeName = "date")]
        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? ReturnTime { get; set; }

        [Required]
        public decimal Fare { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableSeats { get; set; }

        [Required]
        public int FlightOwnerId { get; set; }

        [Required]
        public int RouteId { get; set; }

        public bool IsRoundTrip { get; set; } // True if this flight is part of a roundtrip

        public int? ReturnFlightId { get; set; } // Foreign key for the return flight, nullable

        // Navigation Properties
        [JsonIgnore]
        public virtual FlightOwner FlightOwner { get; set; }
        [JsonIgnore]
        public virtual Routes Route { get; set; }
        [JsonIgnore]
        public virtual ICollection<Seats> Seats { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }
        public virtual BaggageInfos BaggageInfo { get; set; }

        [ForeignKey("ReturnFlightId")]
        public virtual Flights ReturnFlight { get; set; } // For the return flight if applicable
    }
}
