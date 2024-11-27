using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SkyHub.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using SkyHub.DTOs;

using SkyHub.Models;
using SkyHub.Models.Roles;

namespace SkyHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SkyHubDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SkyHubDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Registration endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registrationDto.Email))
                return BadRequest("Email is already registered.");

            // Hash the password
            using var hmac = new HMACSHA256();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registrationDto.Password));
            var passwordSalt = hmac.Key;

            // Create user entity
            var user = new Users
            {
                UserName = registrationDto.UserName,
                Email = registrationDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleType = registrationDto.RoleType,
                DateJoined = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Add additional details based on role
            if (registrationDto.RoleType == "Customer")
            {
                var passenger = new Passenger
                {
                    UserId = user.UserId,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    DOB = registrationDto.DOB,
                    Gender = registrationDto.Gender,
                    PhoneNumber = registrationDto.PhoneNumber,
                    StreetAddress = registrationDto.StreetAddress,
                    City = registrationDto.City,
                    State = registrationDto.State,
                    PostalCode = registrationDto.PostalCode,
                    Country = registrationDto.Country
                };

                _context.Passenger.Add(passenger);
            }
            else if (registrationDto.RoleType == "FlightOwner")
            {
                var flightOwner = new FlightOwner
                {
                    UserId = user.UserId,
                    FirstName = registrationDto.FirstName,
                    LastName = registrationDto.LastName,
                    Gender = registrationDto.Gender,
                    PhoneNumber = registrationDto.PhoneNumber,
                    CompanyName = registrationDto.CompanyName
                };

                _context.FlightOwner.Add(flightOwner);
            }

            await _context.SaveChangesAsync();
            return Ok("Registration successful.");
        }

        // Login endpoint with JWT
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.RoleType == loginDto.RoleType && u.UserName == loginDto.UserName);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            using var hmac = new HMACSHA256(user.PasswordSalt); // Use stored salt
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });

            // try
            // {
            //     
            // }
            // catch (Exception ex)
            // {
            //     return StatusCode(500, "An error occurred during login.");
            // }
        }

        // Generate JWT Token
        private string GenerateJwtToken(Users user)
        {
            var secretKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("JWT Secret Key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
   {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, user.RoleType) // Add the role claim
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
