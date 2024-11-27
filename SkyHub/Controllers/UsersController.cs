using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyHub.Data;
using SkyHub.Models;


namespace SkyHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SkyHubDbContext _context;

        public UsersController(SkyHubDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Validate input
            if (
                //string.IsNullOrWhiteSpace(loginRequest.RoleType) ||
                string.IsNullOrWhiteSpace(loginRequest.Email) ||
                string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                return BadRequest(new { message = "All fields are required." });
            }

            // Validate RoleType
            var validRoles = new[] { "Customer", "FlightOwner", "Admin" };
          //  if (!validRoles.Contains(loginRequest.RoleType, StringComparer.OrdinalIgnoreCase))
          //  {
          //      return BadRequest(new { message = "Invalid role type selected." });
          //  }

            // Find user by email
            var user = _context.Users.SingleOrDefault(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Validate RoleType matches
          //  if (!string.Equals(user.RoleType, loginRequest.RoleType, StringComparison.OrdinalIgnoreCase))
          //  {
          //      return BadRequest(new { message = "The selected role does not match your account." });
          //  }

            // Verify password
            if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Authentication successful, redirect based on role
            switch (user.RoleType)
            {
                case "Customer":
                    return RedirectToAction("CustomerDashboard", "Customer", new { userId = user.UserId });

                case "FlightOwner":
                    return RedirectToAction("FlightOwnerDashboard", "FlightOwner", new { userId = user.UserId });

                case "Admin":
                    return RedirectToAction("AdminDashboard", "Admin", new { userId = user.UserId });

                default:
                    return BadRequest(new { message = "Unexpected role type." });
            }
        }

        private bool VerifyPassword(string password, byte[] storedHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }


    }
}
