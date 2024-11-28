namespace SkyHub.DTOs
{
    public class BookingDto
    {
        // Existing properties...
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public int NumSeats { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public int NumInfants { get; set; }
        public string BookingStatus { get; set; } = "Confirmed";
        public DateTime? BookingDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BookingItemDTO> BookingItems { get; set; } = new();
        public List<SeatTypeDTO> SeatTypes { get; set; } = new();

        // Validation for NumSeats = NumAdults + NumChildren
        public bool IsValidNumSeats()
        {
            return NumSeats == (NumAdults + NumChildren + NumInfants);
        }


    }
    public class BookingItemDTO
    {
        public int SeatId { get; set; }
        public int SeatTypeId { get; set; }
        public decimal Price { get; set; }
    }

    public class SeatTypeDTO
    {
        public int SeatTypeId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string PassengerType { get; set; }
        public string SeatTypeName { get; set; }
        public decimal BaseFare { get; set; }
    }

}
