using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Title must be at least 3 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "The Description must be at least 10 characters.")]
        public string Description { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "The Price must be more than 0")]
        public double Price { get; set; }
     
        public DateTime EndDate { get; set; }
        public bool IsSold { get; set; } = false;
        [Required]
        public string? IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public User? User { get; set; }

    }
}
