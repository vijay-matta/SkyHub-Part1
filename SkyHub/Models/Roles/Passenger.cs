using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SkyHub.Models.Roles
{
    public class Passenger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int? PassengerId { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required, StringLength(100)]
        public string? FirstName { get; set; }

        [Required, StringLength(100)]
        public string? LastName { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Required, StringLength(10)]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Invalid Gender.")]
        public string? Gender { get; set; }

        [Required, StringLength(10)]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(255)]
        public string? StreetAddress { get; set; }

        [Required, StringLength(100)]
        public string? City { get; set; }

        [Required, StringLength(100)]
        public string? State { get; set; }

        [Required, StringLength(20)]
        public string? PostalCode { get; set; }

        [Required, StringLength(100)]
        public string? Country { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public virtual Users User { get; set; }
    }
}
