using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Reflection;

namespace Project.Data.Services
{
    public class BidsService : IBidsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public BidsService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Add(Bid bid)
        {
            var user = await _userManager.FindByIdAsync(bid.IdentityUserId);

            
            var username = user;
            bid.User = username;
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
             
        }
        public async Task RemoveWallet(double price, string userId)
        {

                var bidToUpdate = _context.UserCredit.Where(x => x.IdentityUserId == userId);
                bidToUpdate.FirstOrDefault().Ammount -= price;
                _context.UserCredit.Update(bidToUpdate.FirstOrDefault());
                await _context.SaveChangesAsync();
        }
        public async Task CreditsToOwner(double price, string userId)
        {

            var bidToUpdate = _context.UserCredit.Where(x => x.IdentityUserId == userId);
            bidToUpdate.FirstOrDefault().Ammount += price;
            _context.UserCredit.Update(bidToUpdate.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        public IQueryable<Bid> GetAll()
        {
            var applicationDbContext = from a in _context.Bids.Include(l => l.AuctionList).ThenInclude(l => l.User)
                                       select a;
            return applicationDbContext;
        }
    }
}
