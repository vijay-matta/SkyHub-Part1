using SkyHub.Models.Flight_Details;
using SkyHub.Models.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyHub.Data;

namespace SkyHub.Services
{
    public class FlightService : IFlightService
    {
        private readonly SkyHubDbContext _context;

        public FlightService(SkyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flights>> GetAllFlightsAsync()
        {
            return await _context.Flights.Include(f => f.Route).ToListAsync();
        }

        public async Task<Flights> GetFlightByIdAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Route)
                .Include(f => f.Bookings)
                .FirstOrDefaultAsync(f => f.FlightId == id);
        }

        public async Task<Flights> CreateFlightAsync(Flights flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task<Flights> UpdateFlightAsync(int id, Flights flight)
        {
            var existingFlight = await _context.Flights.FindAsync(id);
            if (existingFlight == null) return null;

            existingFlight.FlightNumber = flight.FlightNumber;
            existingFlight.FlightName = flight.FlightName;
            existingFlight.DepartureTime = flight.DepartureTime;
            existingFlight.ArrivalTime = flight.ArrivalTime;
            existingFlight.DepartureDate = flight.DepartureDate;
            existingFlight.ArrivalDate = flight.ArrivalDate;
            existingFlight.Fare = flight.Fare;
            existingFlight.TotalSeats = flight.TotalSeats;
            existingFlight.AvailableSeats = flight.AvailableSeats;

            await _context.SaveChangesAsync();
            return existingFlight;
        }

        public async Task<bool> DeleteFlightAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return false;

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Bookings>> GetBookingsForFlightAsync(int flightId)
        {
            return await _context.Bookings.Where(b => b.FlightId == flightId).ToListAsync();
        }

        public async Task<Bookings> CreateBookingForFlightAsync(int flightId, Bookings booking)
        {
            var flight = await _context.Flights.FindAsync(flightId);
            if (flight == null) return null;

            booking.FlightId = flightId;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
    }
}
