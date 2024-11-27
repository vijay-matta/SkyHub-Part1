using SkyHub.Models.Flight_Details;
using SkyHub.Models.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyHub.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<Flights>> GetAllFlightsAsync();
        Task<Flights> GetFlightByIdAsync(int id);
        Task<Flights> CreateFlightAsync(Flights flight);
        Task<Flights> UpdateFlightAsync(int id, Flights flight);
        Task<bool> DeleteFlightAsync(int id);
        Task<IEnumerable<Bookings>> GetBookingsForFlightAsync(int flightId);
        Task<Bookings> CreateBookingForFlightAsync(int flightId, Bookings booking);
    }
}
