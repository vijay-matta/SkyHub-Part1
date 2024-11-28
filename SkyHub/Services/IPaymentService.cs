using SkyHub.DTOs;

namespace SkyHub.Services
{
    public class IPaymentService
    {
        public async Task<PaymentDto> ProcessPayment(PaymentDto paymentDTO)
        {
            // Implement logic here
            return new PaymentDto();
        }

        public async Task<RefundDTO> ProcessRefund(int paymentId, bool isBookingCancelled)
        {
            // Implement logic here
            return new RefundDTO();
        }

        public async Task<bool> UpdatePayment(int paymentId, PaymentDto updatedPayment)
        {
            // Implement logic here
            return true;
        }

        public async Task<PaymentDto> GetProcessedPayment(int paymentId)
        {
            // Implement logic here
            return new PaymentDto();
        }

        public async Task<RefundDTO> GetProcessedRefund(int refundId)
        {
            // Implement logic here
            return new RefundDTO();
        }

        public async Task<bool> UpdateRefund(int refundId, RefundDTO updatedRefund)
        {
            // Implement logic here
            return true;
        }

        public async Task<bool> BookingPayment(decimal totalPrice)
        {
            // Implement logic here
            return true;
        }
    }
}
