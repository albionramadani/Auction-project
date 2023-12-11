using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Reflection;

namespace Project.Data.Services
{
    public class UserCreditService : IUserCreditService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserCreditService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreditForNewUser(string uca)
        {
            var user = await _userManager.FindByIdAsync(uca);
            UserCreditAmmount ucas = new UserCreditAmmount();
            if (user != null)
            {
                ucas.IdentityUserId = user.Id;
                ucas.Ammount = 1000.00;
            }

            _context.UserCredit.Add(ucas);
            await _context.SaveChangesAsync();

        }
        public async Task TransferFunds(string fromUserId, string toUserId, double amount)
        {
            // Assuming you have methods to get and update user credits
            var fromUserCredit = _context.UserCredit.FirstOrDefault(u => u.IdentityUserId == fromUserId);
            var toUserCredit = _context.UserCredit.FirstOrDefault(u => u.IdentityUserId == toUserId);

            if (fromUserCredit != null && toUserCredit != null)
            {
                // Deduct amount from the bidder's wallet
                fromUserCredit.Ammount -= amount;

                // Add amount to the auction poster's wallet
                toUserCredit.Ammount += amount;

                // Update user credits
                _context.UserCredit.UpdateRange(fromUserCredit, toUserCredit);

                await _context.SaveChangesAsync();
            }
        }

        public double CreditById(string id)
        {
            var creadits= _context.UserCredit.FirstOrDefault(x=>x.IdentityUserId == id).Ammount;

                return creadits;
        }


    }
}