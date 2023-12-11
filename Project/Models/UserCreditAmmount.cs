using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserCreditAmmount
    {
        public int Id { get; set; }
        [Required]
        public string? IdentityUserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public double Ammount { get; set; } = 1000.00;
    }
}
