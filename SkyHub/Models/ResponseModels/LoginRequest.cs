using SkyHub.Models;

namespace SkyHub.Models.ResponseModels
{
    public class LoginRequest
    {
        public string? RoleType { get; set; } // "Customer", "FlightOwner", or "Admin"
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
