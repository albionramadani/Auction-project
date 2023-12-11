using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Project.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public double Price { get; set; }

        [Required]
        public string? IdentityUserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public int? ListingId { get; set; }
        [ForeignKey("ListingId")]
        public AuctionList? AuctionList { get; set; }
    }
}
