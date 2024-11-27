using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SkyHub.Models.Roles
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int AdminId { get; set; }

        [Required]
        public int UserId { get; set; }

        // Navigation properties
        public Users User { get; set; }
    }
}
