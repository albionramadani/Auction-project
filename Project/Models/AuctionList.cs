using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class AuctionList
    {
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Title must be at least 3 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "The Description must be at least 10 characters.")]
        public string Description { get; set; }
        [Range(1000.00, double.MaxValue, ErrorMessage = "The Price must be at least 1000.00")]  
        public double Price { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsSold { get; set; } = false;

        [Required]
        public string? IdentityUserId { get; set; }
        [ForeignKey("UserId")]
        public string Username { get; set; }
        public User? User { get; set; }
        public List<Bid>? Bids { get; set; }
        public double? UserCreditAmmountId { get; set; }
        public UserCreditAmmount? credits { get; set; }
    }
}
