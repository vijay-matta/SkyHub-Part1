using SkyHub.Data;
using SkyHub.Models.Roles;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkyHub.Models.Flight_Details;
using SkyHub.DTOs;

namespace SkyHub.Services
{
    

    public class BookingService : IBookingService
    {
        private readonly SkyHubDbContext _context;
        private readonly IPaymentService _paymentService;

        public BookingService(SkyHubDbContext context, IPaymentService paymentService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context cannot be null.");
            _paymentService = paymentService;
        }

        public IEnumerable<BookingDto> GetAllBookings()
        {
            var bookings = _context.Bookings
                .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.SeatType)
                .Select(b => new BookingDto
                {
                    UserId = b.UserId,
                    FlightId = b.FlightId,
                    NumSeats = b.NumSeats,
                    NumAdults = b.NumAdults,
                    NumChildren = b.NumChildren,
                    NumInfants = b.NumInfants,
                    BookingStatus = b.BookingStatus,
                    BookingDate = b.BookingDate,
                    CancelDate = b.CancelDate,
                    TotalPrice = b.TotalPrice,
                    BookingItems = b.BookingItems.Select(bi => new BookingItemDTO
                    {
                        SeatId = bi.SeatId,
                        SeatTypeId = bi.SeatTypeId,
                        Price = bi.Price
                    }).ToList(),
                    SeatTypes = b.BookingItems.Select(bi => new SeatTypeDTO
                    {
                        SeatTypeId = bi.SeatTypeId,
                        Name = bi.SeatType.Name,
                        Age = bi.SeatType.Age,
                        PassengerType = bi.SeatType.PassengerType,
                        SeatTypeName = bi.SeatType.SeatTypeName,
                        BaseFare = bi.SeatType.BaseFare
                    }).ToList()
                });

            return bookings.ToList();
        }

        public async Task ProcessBooking(BookingDto bookingDTO)
        {
            bool paymentSuccess = await _paymentService.BookingPayment(bookingDTO.TotalPrice);
            // Continue with logic
        }



        public async Task<int> CreateBooking(BookingDto bookingDTO) // Mark as async
        {
            if (!bookingDTO.IsValidNumSeats())
            {
                throw new ArgumentException("Number of seats must equal the sum of adults and children.");
            }

            if (bookingDTO == null)
                throw new ArgumentNullException(nameof(bookingDTO), "BookingDTO cannot be null.");

            if (bookingDTO.BookingItems == null || !bookingDTO.BookingItems.Any())
                throw new ArgumentException("BookingItems cannot be null or empty.");

            if (bookingDTO.SeatTypes == null || !bookingDTO.SeatTypes.Any())
                throw new ArgumentException("SeatTypes cannot be null or empty.");

            using var transaction = _context.Database.BeginTransaction(); // Use a transaction for atomicity
            try
            {
                // Create Booking with Pending status by default
                var booking = new Bookings
                {
                    UserId = bookingDTO.UserId,
                    FlightId = bookingDTO.FlightId,
                    BookingDate = DateTime.Now,
                    NumSeats = bookingDTO.NumSeats,
                    NumAdults = bookingDTO.NumAdults,
                    NumChildren = bookingDTO.NumChildren,
                    NumInfants = bookingDTO.NumInfants,
                    BookingStatus = "Pending",  // Set status to Pending initially
                    TotalPrice = bookingDTO.TotalPrice
                };

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                foreach (var item in bookingDTO.BookingItems)
                {
                    var bookingItem = new BookingItems
                    {
                        BookingId = booking.BookingId,
                        SeatId = item.SeatId,
                        SeatTypeId = item.SeatTypeId,
                        Price = item.Price
                    };
                    _context.BookingItems.Add(bookingItem);

                    // Update Seat Availability
                    var seat = _context.Seats.FirstOrDefault(s => s.SeatId == item.SeatId);
                    if (seat != null)
                    {
                        seat.IsAvailable = false;
                        _context.Seats.Update(seat);
                    }
                }

                // Update SeatType Booking Counts
                foreach (var seatType in bookingDTO.SeatTypes)
                {
                    var seatTypeEntity = _context.SeatTypes.FirstOrDefault(st => st.SeatTypeId == seatType.SeatTypeId);
                    if (seatTypeEntity != null)
                    {
                        seatTypeEntity.BaseFare += booking.TotalPrice; // Example: Increment revenue for the seat type
                        _context.SeatTypes.Update(seatTypeEntity);
                    }
                }

                _context.SaveChanges();
                transaction.Commit(); // Commit the transaction

                // Use ProcessPayment to process the payment (awaiting the asynchronous call)
                bool paymentSuccess = await _paymentService.BookingPayment(bookingDTO.TotalPrice); // Use await here
                if (paymentSuccess)
                {
                    booking.BookingStatus = "Confirmed"; // Update status to Confirmed if payment is successful
                    _context.Bookings.Update(booking);
                    _context.SaveChanges();
                }
                else
                {
                    // If payment fails, leave status as Pending
                    return booking.BookingId;
                }

                return booking.BookingId;
            }
            catch
            {
                transaction.Rollback(); // Rollback in case of an error
                throw;
            }
        }



        public BookingDto GetBookingById(int bookingId)
        {
            var booking = _context.Bookings
                .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.SeatType)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null) throw new KeyNotFoundException("Booking not found.");

            return new BookingDto
            {
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                NumSeats = booking.NumSeats,
                NumAdults = booking.NumAdults,
                NumChildren = booking.NumChildren,
                NumInfants = booking.NumInfants,
                BookingStatus = booking.BookingStatus,
                BookingDate = booking.BookingDate,
                CancelDate = booking.CancelDate,
                TotalPrice = booking.TotalPrice,
                BookingItems = booking.BookingItems.Select(bi => new BookingItemDTO
                {
                    SeatId = bi.SeatId,
                    SeatTypeId = bi.SeatTypeId,
                    Price = bi.Price
                }).ToList()
            };
        }

        public bool CancelBooking(int bookingId)
        {
            using var transaction = _context.Database.BeginTransaction(); // Use a transaction for atomicity
            try
            {
                var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

                if (booking == null || booking.BookingStatus == "Cancelled")
                    return false;

                // If the status is Pending, directly cancel it
                if (booking.BookingStatus == "Pending")
                {
                    booking.BookingStatus = "Cancelled";
                    booking.CancelDate = DateTime.Now;
                    _context.Bookings.Update(booking);
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }

                // Proceed with normal cancellation for confirmed bookings
                booking.BookingStatus = "Cancelled";
                booking.CancelDate = DateTime.Now;
                _context.Bookings.Update(booking);

                // Update Seat Availability and SeatType Booking Counts
                var bookingItems = _context.BookingItems.Where(bi => bi.BookingId == bookingId).ToList();
                foreach (var item in bookingItems)
                {
                    var seat = _context.Seats.FirstOrDefault(s => s.SeatId == item.SeatId);
                    if (seat != null)
                    {
                        seat.IsAvailable = true;
                        _context.Seats.Update(seat);
                    }

                    // Optionally, handle seat type changes or adjustments
                    var seatType = _context.SeatTypes.FirstOrDefault(st => st.SeatTypeId == item.SeatTypeId);
                    if (seatType != null)
                    {
                        seatType.BaseFare -= item.Price; // Example: Adjust revenue for the seat type
                        _context.SeatTypes.Update(seatType);
                    }
                }

                _context.SaveChanges();
                transaction.Commit(); // Commit the transaction
                return true;
            }
            catch
            {
                transaction.Rollback(); // Rollback in case of an error
                throw;
            }
        }



        //public void DeleteBooking(int bookingId)
        //{
        //    var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        //    if (booking == null) throw new KeyNotFoundException("Booking not found.");

        //    _context.Bookings.Remove(booking);
        //    _context.SaveChanges();
        //}

        public List<BookingDto> GetBookingHistory(int userId)
        {
            return _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.BookingItems)
                .ThenInclude(bi => bi.SeatType)
                .Select(b => new BookingDto
                {
                    UserId = b.UserId,
                    FlightId = b.FlightId,
                    NumSeats = b.NumSeats,
                    NumAdults = b.NumAdults,
                    NumChildren = b.NumChildren,
                    NumInfants = b.NumInfants,
                    BookingStatus = b.BookingStatus,
                    TotalPrice = b.TotalPrice,
                    BookingItems = b.BookingItems.Select(bi => new BookingItemDTO
                    {
                        SeatId = bi.SeatId,
                        SeatTypeId = bi.SeatTypeId,
                        Price = bi.Price
                    }).ToList()
                })
                .ToList();
        }

    }
}
