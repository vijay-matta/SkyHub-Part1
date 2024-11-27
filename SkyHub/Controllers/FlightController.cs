using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyHub.Data;
using SkyHub.DTOs;
using SkyHub.Models.Flight_Details;
using SkyHub.Models.Roles;
using SkyHub.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,FlightOwner")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly SkyHubDbContext _context;
        private readonly IConfiguration _configuration;

        // Constructor that injects both IFlightService and SkyHubDbContext
        public FlightController(IFlightService flightService, SkyHubDbContext context, IConfiguration configuration)
        {
            _flightService = flightService;
            _context = context;
            _configuration = configuration;
        }

        // GET: /api/flights
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Flights>>> GetFlights()
        {
            try
            {
                var flights = await _flightService.GetAllFlightsAsync();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: /api/flights/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Flights>> GetFlight(int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        // POST: /api/flights
        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody] FlightDto flightDto)
        {
            // Validate the incoming data
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            // Check if the FlightOwner exists
            var flightOwnerExists = await _context.FlightOwner.FindAsync(flightDto.FlightOwnerId);
            if (flightOwnerExists == null)
            {
                return NotFound(new { message = $"FlightOwner with ID {flightDto.FlightOwnerId} not found." });
            }

            // Check if the Route exists
            var routeExists = await _context.Routes.FindAsync(flightDto.RouteId);
            if (routeExists == null)
            {
                return NotFound(new { message = $"Route with ID {flightDto.RouteId} not found." });
            }

            // Map DTO to Entity
            var flight = new Flights
            {
                FlightNumber = flightDto.FlightNumber ?? "DefaultFlightNumber", // Provide a default value or handle nulls
                FlightName = flightDto.FlightName ?? "DefaultFlightName",       // Provide a default value or handle nulls
                DepartureTime = flightDto.DepartureTime ?? TimeSpan.Zero,       // Default to 00:00:00 if null
                ArrivalTime = flightDto.ArrivalTime ?? TimeSpan.Zero,           // Default to 00:00:00 if null
                DepartureDate = flightDto.DepartureDate ?? DateTime.MinValue,   // Default to MinValue if null
                ArrivalDate = flightDto.ArrivalDate ?? DateTime.MinValue,       // Default to MinValue if null
                Fare = flightDto.Fare ?? 0.0m,                                 // Default to 0.0 if null
                TotalSeats = flightDto.TotalSeats ?? 0,                        // Default to 0 if null
                AvailableSeats = flightDto.AvailableSeats ?? 0,                // Default to 0 if null
                FlightOwnerId = flightDto.FlightOwnerId ?? 0,                  // Default to 0 if null
                RouteId = flightDto.RouteId ?? 0,                              // Default to 0 if null
                IsRoundTrip = flightDto.IsRoundTrip ?? false,                  // Default to false if null
                ReturnDate = flightDto.ReturnDate ?? DateTime.MinValue,        // Default to MinValue if null
                ReturnTime = flightDto.ReturnTime ?? TimeSpan.Zero             // Default to 00:00:00 if null
            };


            try
            {
                // Save to database
                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateFlight), new { id = flight.FlightId }, new
                {
                    message = "Flight created successfully.",
                    flight
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the flight.",
                    details = ex.Message
                });
            }
        }


        // PUT: /api/flights/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] FlightDto flightDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the existing flight by ID
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound(new { message = "Flight not found" });
            }

            // Update properties only if they are provided in the DTO
            if (!string.IsNullOrEmpty(flightDto.FlightNumber))
                flight.FlightNumber = flightDto.FlightNumber;

            if (!string.IsNullOrEmpty(flightDto.FlightName))
                flight.FlightName = flightDto.FlightName;

            if (flightDto.DepartureTime.HasValue)
                flight.DepartureTime = flightDto.DepartureTime.Value;

            if (flightDto.ArrivalTime.HasValue)
                flight.ArrivalTime = flightDto.ArrivalTime.Value;

            if (flightDto.DepartureDate.HasValue)
                flight.DepartureDate = flightDto.DepartureDate.Value;

            if (flightDto.ArrivalDate.HasValue)
                flight.ArrivalDate = flightDto.ArrivalDate.Value;

            if (flightDto.Fare.HasValue)
                flight.Fare = flightDto.Fare.Value;

            if (flightDto.TotalSeats.HasValue)
                flight.TotalSeats = flightDto.TotalSeats.Value;

            if (flightDto.AvailableSeats.HasValue)
                flight.AvailableSeats = flightDto.AvailableSeats.Value;

            if (flightDto.FlightOwnerId.HasValue)
                flight.FlightOwnerId = flightDto.FlightOwnerId.Value;

            if (flightDto.RouteId.HasValue)
                flight.RouteId = flightDto.RouteId.Value;

            if (flightDto.IsRoundTrip.HasValue)
                flight.IsRoundTrip = flightDto.IsRoundTrip.Value;

            if (flightDto.ReturnDate.HasValue)
                flight.ReturnDate = flightDto.ReturnDate.Value;

            if (flightDto.ReturnTime.HasValue)
                flight.ReturnTime = flightDto.ReturnTime.Value;

            // Explicitly mark the flight entity as modified
            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(new { message = "Flight updated successfully", flight });
        }



        // DELETE: /api/flights/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound(new { message = "Flight not found" });
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Flight deleted successfully" });
        }

        // GET: /api/flights/{id}/bookings
        [HttpGet("{id}/bookings")]
        public async Task<ActionResult<IEnumerable<Bookings>>> GetFlightBookings(int id)
        {
            var bookings = await _flightService.GetBookingsForFlightAsync(id);
            if (bookings == null || !bookings.Any()) // Use .Any() to check for empty list
            {
                return NotFound("No bookings found for this flight.");
            }
            return Ok(bookings);
        }

        // POST: /api/flights/{id}/bookings
        [HttpPost("{id}/bookings")]
        public async Task<ActionResult<Bookings>> CreateBookingForFlight(int id, [FromBody] Bookings booking)
        {
            if (booking == null)
            {
                return BadRequest("Invalid booking data.");
            }

            var createdBooking = await _flightService.CreateBookingForFlightAsync(id, booking);
            return CreatedAtAction(nameof(GetFlightBookings), new { id = id }, createdBooking);
        }
    }
}
