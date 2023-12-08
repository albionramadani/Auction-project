 
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Project.Models
{
    public class User : IdentityUser
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required.")]
        //[StringLength(20, MinimumLength = 4, ErrorMessage = "First Name should be longer than three characters and maximum twenty")]
        public string FirstName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required.")]
        //[StringLength(20, MinimumLength = 4, ErrorMessage = "Last Name should be longer than three characters and maximum twenty")]
        public string LastName { get; set; }
        public decimal WalletAmount { get; set; } = 1000.00m;

        public ICollection<Auction> Auctions { get; set; }
    }
}
