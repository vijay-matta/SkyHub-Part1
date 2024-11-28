using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SkyHub.Data;
using SkyHub.Models.Roles;
using SkyHub.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SkyHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Ensure the user is authenticated
    public class UserProfileController : ControllerBase
    {
        private readonly SkyHubDbContext _context;
        private readonly IConfiguration _configuration;

        public UserProfileController(SkyHubDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string GetUserNameFromToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userName = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User not found in the token.");
            }

            return userName;
        }

        // Get the profile of the logged-in user
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userName = GetUserNameFromToken(); // Retrieve UserName from the token
            var user = await _context.Users
                                     .Include(u => u.Customer) // Include Passenger data
                                     .Include(u => u.FlightOwner) // Include FlightOwner data
        
                                      .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userProfile = new UserProfileDto
            {
                //UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                RoleType = user.RoleType
            };

            if (user.RoleType == "Customer" && user.Customer != null)
            {
                userProfile.FirstName = user.Customer.FirstName;
                userProfile.LastName = user.Customer.LastName;
                userProfile.Gender = user.Customer.Gender;
                userProfile.PhoneNumber = user.Customer.PhoneNumber;
                userProfile.StreetAddress = user.Customer.StreetAddress;
                userProfile.City = user.Customer.City;
                userProfile.State = user.Customer.State;
                userProfile.PostalCode = user.Customer.PostalCode;
                userProfile.Country = user.Customer.Country;
            }
            else if (user.RoleType == "FlightOwner" && user.FlightOwner != null)
            {
                userProfile.FirstName = user.FlightOwner.FirstName;
                userProfile.LastName = user.FlightOwner.LastName;
                userProfile.PhoneNumber = user.FlightOwner.PhoneNumber;
                userProfile.CompanyName = user.FlightOwner.CompanyName;
            }
            // Include other properties as necessary


            return Ok(userProfile);
        }

        // Edit the profile of the logged-in user
        [HttpPatch("profile/edit")]
        public async Task<IActionResult> PatchProfile([FromBody] UserProfileDto updatedProfile)
        {
            var userName = GetUserNameFromToken(); // Retrieve UserName from the token
            var user = await _context.Users
                           .Include(u => u.Customer)
                           .Include(u => u.FlightOwner)
                           .FirstOrDefaultAsync(u => u.UserName == userName);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Update user properties only if provided
            if (!string.IsNullOrEmpty(updatedProfile.Email))
                user.Email = updatedProfile.Email;

            if (!string.IsNullOrEmpty(updatedProfile.RoleType))
                user.RoleType = updatedProfile.RoleType;

            // Mark the user as modified explicitly
            _context.Entry(user).State = EntityState.Modified;

            // Update properties for the Customer (Passenger) role if available
            if (user.RoleType == "Passenger" && user.Customer != null)
            {
                if (!string.IsNullOrEmpty(updatedProfile.FirstName))
                    user.Customer.FirstName = updatedProfile.FirstName;

                if (!string.IsNullOrEmpty(updatedProfile.LastName))
                    user.Customer.LastName = updatedProfile.LastName;

                if (!string.IsNullOrEmpty(updatedProfile.Gender))
                    user.Customer.Gender = updatedProfile.Gender;

                if (!string.IsNullOrEmpty(updatedProfile.PhoneNumber))
                    user.Customer.PhoneNumber = updatedProfile.PhoneNumber;

                if (!string.IsNullOrEmpty(updatedProfile.StreetAddress))
                    user.Customer.StreetAddress = updatedProfile.StreetAddress;

                if (!string.IsNullOrEmpty(updatedProfile.City))
                    user.Customer.City = updatedProfile.City;

                if (!string.IsNullOrEmpty(updatedProfile.State))
                    user.Customer.State = updatedProfile.State;

                if (!string.IsNullOrEmpty(updatedProfile.PostalCode))
                    user.Customer.PostalCode = updatedProfile.PostalCode;

                if (!string.IsNullOrEmpty(updatedProfile.Country))
                    user.Customer.Country = updatedProfile.Country;

                // Mark the Customer entity as modified explicitly
                _context.Entry(user.Customer).State = EntityState.Modified;
            }

            // Update properties for the FlightOwner role if available
            else if (user.RoleType == "FlightOwner" && user.FlightOwner != null)
            {
                if (!string.IsNullOrEmpty(updatedProfile.FirstName))
                    user.FlightOwner.FirstName = updatedProfile.FirstName;

                if (!string.IsNullOrEmpty(updatedProfile.LastName))
                    user.FlightOwner.LastName = updatedProfile.LastName;

                if (!string.IsNullOrEmpty(updatedProfile.Gender))
                    user.FlightOwner.Gender = updatedProfile.Gender;

                if (!string.IsNullOrEmpty(updatedProfile.PhoneNumber))
                    user.FlightOwner.PhoneNumber = updatedProfile.PhoneNumber;

                if (!string.IsNullOrEmpty(updatedProfile.CompanyName))
                    user.FlightOwner.CompanyName = updatedProfile.CompanyName;

                // Mark the FlightOwner entity as modified explicitly
                _context.Entry(user.FlightOwner).State = EntityState.Modified;
            }


            // Explicitly mark the entity as modified if needed
            if (user.Customer != null)
            {
                _context.Entry(user).State = EntityState.Modified;

            }

            if (user.FlightOwner != null)
            {
                _context.Entry(user.FlightOwner).State = EntityState.Modified;
            }


            // Save changes to the database
            try
            {
                await _context.SaveChangesAsync();
                return Ok("Profile updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            
        }



        // Delete the profile of the logged-in user
        [HttpDelete("profile/delete")]
        public async Task<IActionResult> DeleteProfile()
        {
            var userName = GetUserNameFromToken(); // Retrieve UserName from the token
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Profile deleted successfully.");
        }

        // Helper method to extract UserId from JWT Token
        private int GetUserIdFromToken()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User is not authenticated.");
            return int.Parse(userIdClaim.Value);
        }
    }
}
