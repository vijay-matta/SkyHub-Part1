using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;



namespace SkyHub.DTOs
{
    public class FlightDto
    {
        public string FlightNumber { get; set; }
        public string FlightName { get; set; }
        //[System.Text.Json.Serialization.JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan? DepartureTime { get; set; }
        //[System.Text.Json.Serialization.JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan? ArrivalTime { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public decimal? Fare { get; set; }
        public int? TotalSeats { get; set; }
        public int? AvailableSeats { get; set; }
        public int? FlightOwnerId { get; set; }
        public int? RouteId { get; set; }
        public bool? IsRoundTrip { get; set; }
        public DateTime? ReturnDate { get; set; }
        public TimeSpan? ReturnTime { get; set; }
    }
}
