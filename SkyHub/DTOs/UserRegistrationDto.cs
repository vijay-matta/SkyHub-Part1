using System.ComponentModel.DataAnnotations;

namespace SkyHub.DTOs
{
    public class UserRegistrationDto : IValidatableObject
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RoleType { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // No [Required] here
        public string CompanyName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleType == "FlightOwner" && string.IsNullOrWhiteSpace(CompanyName))
            {
                yield return new ValidationResult(
                    "The CompanyName field is required for FlightOwner role.",
                    new[] { nameof(CompanyName) });
            }
        }
    }

    public class UserLoginDto
    {
        [Required, RegularExpression("Customer|FlightOwner|Admin", ErrorMessage = "Invalid Role Type.")]
        public string RoleType { get; set; }

        [Required, StringLength(50)]
        public string UserName { get; set; }

        [Required, StringLength(100)]
        public string Password { get; set; }
    }
}
