using SkyHub.Data;
using SkyHub.Models.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyHub.Models.Flight_Details;

namespace SkyHub.Services
{
    

    public class BookingService : IBookingService
    {
        private readonly SkyHubDbContext _context;

        public BookingService(SkyHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bookings>> GetAllBookingsAsync()
        {
            return await _context.Bookings.Include(b => b.User)
                                          .Include(b => b.Flight)
                                          .ToListAsync();
        }

        public async Task<Bookings> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                                 .Include(b => b.User)
                                 .Include(b => b.Flight)
                                 .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task UpdateBookingAsync(int bookingId, Bookings updatedBooking)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking != null)
            {
                booking.NumSeats = updatedBooking.NumSeats;
                booking.BookingStatus = updatedBooking.BookingStatus;
                booking.TotalPrice = updatedBooking.TotalPrice;

                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}
