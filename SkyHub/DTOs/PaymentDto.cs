namespace SkyHub.DTOs
{
    public class PaymentDto
    {
        public int BookingId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMode { get; set; }
        public string TransactionId { get; set; }
    }
    public class RefundDTO
    {
        public int PaymentId { get; set; }
        public decimal RefundAmount { get; set; }
        public string RefundMode { get; set; }
        public string RefundReason { get; set; }
    }
}
