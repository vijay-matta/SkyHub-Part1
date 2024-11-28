using SkyHub.Data;
using SkyHub.DTOs;
using SkyHub.Models.Payment_Details;
using Microsoft.EntityFrameworkCore;


namespace SkyHub.Services
{
    public class PaymentService :IPaymentService
    {
        private readonly SkyHubDbContext _context;

        public PaymentService(SkyHubDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentDto> ProcessPayment(PaymentDto paymentDTO)
        {
            decimal tax = paymentDTO.TotalPrice * 0.12M;
            decimal handlingCharges = paymentDTO.TotalPrice * 0.28M;
            paymentDTO.AmountPaid = paymentDTO.TotalPrice + tax + handlingCharges;

            var payment = new Payments
            {
                BookingId = paymentDTO.BookingId,
                AmountPaid = paymentDTO.AmountPaid,
                PaymentStatus = "Pending", // Assume "Pending" initially
                PaymentMode = paymentDTO.PaymentMode,
                TransactionId = paymentDTO.TransactionId
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // After payment is successfully processed, update PaymentStatus to "Success"
            payment.PaymentStatus = "Success";
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            paymentDTO.PaymentStatus = "Success"; // After processing, assume success
            return paymentDTO;
        }

        public async Task<RefundDTO> ProcessRefund(int paymentId, bool isBookingCancelled)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment == null)
                throw new Exception("Payment not found.");

            decimal refundAmount = 0;
            string refundReason = string.Empty;

            if (isBookingCancelled)
            {
                refundAmount = payment.AmountPaid * 0.50M;  // 50% refund for booking cancellation
                refundReason = "Booking Cancelled";
            }
            else if (payment.PaymentStatus == "Failed")
            {
                refundAmount = payment.AmountPaid;  // Full refund in case of payment failure
                refundReason = "Payment Failed";
            }

            var refund = new Refunds
            {
                PaymentId = paymentId,
                RefundAmount = refundAmount,
                RefundMode = "Bank Transfer", // Assume refund mode
                RefundReason = refundReason
            };

            _context.Refunds.Add(refund);
            await _context.SaveChangesAsync();

            // After refund is processed, update PaymentStatus to "Refunded"
            payment.PaymentStatus = "Refunded";
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return new RefundDTO
            {
                PaymentId = paymentId,
                RefundAmount = refundAmount,
                RefundMode = refund.RefundMode,
                RefundReason = refundReason
            };
        }


        // Get Processed Payment by PaymentId
        public async Task<PaymentDto> GetProcessedPayment(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)  // Assuming Booking entity is related
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null) return null;

            return new PaymentDto
            {
                BookingId = payment.BookingId,
                TotalPrice = payment.AmountPaid / 1.40M,  // Reverse calculation to get original TotalPrice
                AmountPaid = payment.AmountPaid,
                PaymentStatus = payment.PaymentStatus,
                PaymentMode = payment.PaymentMode,
                TransactionId = payment.TransactionId
            };
        }

        public async Task<bool> UpdatePayment(int paymentId, PaymentDto updatedPayment)
        {
            var existingPayment = await _context.Payments.FindAsync(paymentId);
            if (existingPayment == null)
                return false;

            existingPayment.PaymentMode = updatedPayment.PaymentMode;
            existingPayment.PaymentStatus = updatedPayment.PaymentStatus;
            existingPayment.TransactionId = updatedPayment.TransactionId;
            existingPayment.AmountPaid = updatedPayment.AmountPaid;

            _context.Payments.Update(existingPayment);
            await _context.SaveChangesAsync();
            return true;
        }


        // Get Processed Refund by RefundId
        public async Task<RefundDTO> GetProcessedRefund(int refundId)
        {
            var refund = await _context.Refunds
                .Include(r => r.Payment)  // Assuming Payment entity is related
                .FirstOrDefaultAsync(r => r.RefundId == refundId);

            if (refund == null) return null;

            return new RefundDTO
            {
                PaymentId = refund.PaymentId,
                RefundAmount = refund.RefundAmount,
                RefundMode = refund.RefundMode,
                RefundReason = refund.RefundReason
            };
        }


        public async Task<bool> UpdateRefund(int refundId, RefundDTO updatedRefund)
        {
            var existingRefund = await _context.Refunds.FindAsync(refundId);
            if (existingRefund == null)
                return false;

            existingRefund.RefundMode = updatedRefund.RefundMode;
            existingRefund.RefundReason = updatedRefund.RefundReason;
            existingRefund.RefundAmount = updatedRefund.RefundAmount;

            _context.Refunds.Update(existingRefund);
            await _context.SaveChangesAsync();
            return true;
        }


        public bool BookingPayment(decimal totalPrice)
        {
            // For demonstration purposes, assuming the payment is always successful.
            // In a real-world scenario, this would involve actual payment gateway integration.
            // We can replace this with actual API calls to payment providers.

            if (totalPrice <= 0)
            {
                return false; // Invalid price
            }

            // For now, let's assume payment is always successful
            return true;
        }
    }
}
