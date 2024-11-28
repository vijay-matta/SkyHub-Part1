using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Payment_Details
{
    public class Refunds
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int RefundId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Refund amount must be greater than or equal to 0.")]
        public decimal RefundAmount { get; set; }

        [Required, StringLength(50)]
        public string RefundMode { get; set; }

        [Required]
        public DateTime RefundDate { get; set; } = DateTime.Now;

        [Required, StringLength(100)]
        public string RefundReason { get; set; }

        // Navigation Properties
        public virtual Payments Payment { get; set; }
    }
}
