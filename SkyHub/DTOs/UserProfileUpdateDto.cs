namespace SkyHub.DTOs
{
    public class UserProfileUpdateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        // Common fields for both Customer and FlightOwner
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        // Specific fields for FlightOwner
        public string CompanyName { get; set; }
    }
}
