using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SkyHub.Models.Flight_Details;
using Microsoft.AspNetCore.Identity;


namespace SkyHub.Models.Roles
{
    public class Users 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensures IDENTITY(1,1)
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[!@#$%^&*])(?=.*[A-Z]).*$",
            ErrorMessage = "Password must be at least 8 characters long, include an uppercase letter, a number, and a special character.")]
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [Required, StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("Customer|FlightOwner|Admin", ErrorMessage = "Invalid Role Type.")]
        public string RoleType { get; set; }

        [Required]
        public DateTime DateJoined { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Passenger Customer { get; set; }
        public virtual FlightOwner FlightOwner { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual ICollection<Bookings> Bookings { get; set; }
    }
}
