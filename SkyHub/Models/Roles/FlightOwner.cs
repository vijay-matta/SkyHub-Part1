using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkyHub.Models.Flight_Details;
using System.Text.Json.Serialization;

namespace SkyHub.Models.Roles
{
    public class FlightOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int? FlightOwnerId { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }

        [Required, StringLength(10)]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Invalid Gender.")]
        public string? Gender { get; set; }

        [Required, StringLength(10)]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(100)]
        public string? CompanyName { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Users User { get; set; }
        [JsonIgnore]
        public ICollection<Routes> Route { get; set; }
        public ICollection<Flights> Flights { get; set; }
    }
}
