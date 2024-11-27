using SkyHub.Models.Roles;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkyHub.Models.Flight_Details
{
    public class Routes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int RouteId { get; set; }

        [Required, StringLength(100)]
        public string Origin { get; set; }

        [Required, StringLength(100)]
        public string Destination { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public decimal Distance { get; set; }

        [Required]
        public int FlightOwnerId { get; set; }

        // Navigation Properties
        public FlightOwner FlightOwner { get; set; }
        [JsonIgnore]
        public ICollection<Flights> Flights { get; set; }

    }
}
