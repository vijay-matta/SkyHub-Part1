using Microsoft.AspNetCore.Mvc;
using SkyHub.Services;
using SkyHub.Models.Roles;
using SkyHub.Models.Flight_Details;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyHub.Controllers
{
    [ApiController]
    [Route("api/admin")]
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

        // Get all bookings
        [HttpGet("bookings")]
        public async Task<ActionResult<IEnumerable<Bookings>>> GetBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // Get a booking by ID
        [HttpGet("bookings/{id}")]
        public async Task<ActionResult<Bookings>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        // Update a booking by ID
        [HttpPut("bookings/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Bookings updatedBooking)
        {
            await _bookingService.UpdateBookingAsync(id, updatedBooking);
            return NoContent();
        }

        // Delete a booking by ID
        [HttpDelete("bookings/{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            await _bookingService.DeleteBookingAsync(id);
            return NoContent();
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
