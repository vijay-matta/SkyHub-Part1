using SkyHub.DTOs;
using SkyHub.Models.Flight_Details;

namespace SkyHub.Services
{
    public interface IBookingService
    {
        IEnumerable<BookingDto> GetAllBookings();
        Task<int> CreateBooking(BookingDto bookingDTO);
        BookingDto GetBookingById(int bookingId);
        bool CancelBooking(int bookingId);

        //void DeleteBooking(int bookingId);
        List<BookingDto> GetBookingHistory(int userId);
    }
}
