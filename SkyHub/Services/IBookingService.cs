using SkyHub.Models.Flight_Details;

namespace SkyHub.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Bookings>> GetAllBookingsAsync();
        Task<Bookings> GetBookingByIdAsync(int bookingId);
        Task UpdateBookingAsync(int bookingId, Bookings updatedBooking);
        Task DeleteBookingAsync(int bookingId);
    }
}
