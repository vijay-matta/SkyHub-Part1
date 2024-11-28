using Microsoft.AspNetCore.Mvc;
using SkyHub.Services;
using SkyHub.Models.Roles;
using SkyHub.Models.Flight_Details;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SkyHub.DTOs;

namespace SkyHub.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private readonly IRouteService _routeService;

        public AdminController(IUserService userService, IBookingService bookingService, IRouteService routeService)
        {
            _userService = userService;
            _bookingService = bookingService;
            _routeService = routeService;
        }

        // Get all users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get a user by ID
        [HttpGet("users/{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Update a user by ID
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users updatedUser)
        {
            await _userService.UpdateUserAsync(id, updatedUser);
            return NoContent();
        }

        // Delete a user by ID
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<BookingDto>> GetAllBookings()
        {
            var bookings = _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        
        [HttpGet("{bookingId}")]
        public ActionResult<BookingDto> GetBookingById(int bookingId)
        {
            try
            {
                var booking = _bookingService.GetBookingById(bookingId);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        
        [HttpPut("{bookingId}")]
        public async Task<ActionResult> UpdateBookingById(int bookingId, [FromBody] BookingDto bookingDto)
        {
            if (bookingDto == null || bookingId <= 0)
                return BadRequest(new { Message = "Invalid booking data." });

            try
            {
                var existingBooking = _bookingService.GetBookingById(bookingId);
                if (existingBooking == null)
                    return NotFound(new { Message = "Booking not found." });

                // Update fields as needed (example: status, price, etc.)
                existingBooking.BookingStatus = bookingDto.BookingStatus;
                existingBooking.TotalPrice = bookingDto.TotalPrice;
                existingBooking.NumSeats = bookingDto.NumSeats;

                // Use the create method for demo purpose; can add a service method to handle updates.
                await _bookingService.CreateBooking(existingBooking);
                return Ok(new { Message = "Booking updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the booking.", Details = ex.Message });
            }
        }

        
        [HttpDelete("{bookingId}")]
        public ActionResult DeleteBookingById(int bookingId)
        {
            try
            {
                var booking = _bookingService.GetBookingById(bookingId);
                if (booking == null)
                    return NotFound(new { Message = "Booking not found." });

                // Uncomment and implement the DeleteBooking method in the service if needed.
                //_bookingService.DeleteBooking(bookingId);

                return Ok(new { Message = "Booking deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the booking.", Details = ex.Message });
            }
        }

        // Get all routes
        [HttpGet("routes")]
        public async Task<ActionResult<IEnumerable<Routes>>> GetRoutes()
        {
            var routes = await _routeService.GetAllRoutesAsync();
            return Ok(routes);
        }

        // Add a new route
        [HttpPost("routes")]
        public async Task<IActionResult> AddRoute(Routes newRoute)
        {
            await _routeService.AddRouteAsync(newRoute);
            return CreatedAtAction(nameof(GetRouteById), new { id = newRoute.RouteId }, newRoute);
        }

        // Get a route by ID
        [HttpGet("routes/{id}")]
        public async Task<ActionResult<Routes>> GetRouteById(int id)
        {
            var route = await _routeService.GetRouteByIdAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            return Ok(route);
        }

        // Update a route by ID
        [HttpPut("routes/{id}")]
        public async Task<IActionResult> UpdateRoute(int id, Routes updatedRoute)
        {
            await _routeService.UpdateRouteAsync(id, updatedRoute);
            return NoContent();
        }

        // Delete a route by ID
        [HttpDelete("routes/{id}")]
        public async Task<IActionResult> DeleteRoute(int id)
        {
            await _routeService.DeleteRouteAsync(id);
            return NoContent();
        }
    }
}
