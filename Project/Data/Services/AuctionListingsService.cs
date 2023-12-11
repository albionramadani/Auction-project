
using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Data.Services
{
    public class AuctionListingsService : IActionListingsService
    {
        private readonly ApplicationDbContext _context;

        public AuctionListingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(AuctionList listing)
        {
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();
        }

        public IQueryable<AuctionList> GetAll()
        {
            var applicationDbContext = _context.Listings.Include(l => l.User).Where(x=>x.EndDate > DateTime.Now);
            return applicationDbContext;
        }
        public IQueryable<AuctionList> GetAllAuctionsWithCredits(string currentUserId)
        {

            var currentcredit=_context.UserCredit.FirstOrDefault(credit => credit.IdentityUserId == currentUserId);
            var query = from auction in _context.Listings
                        join credit in _context.UserCredit on auction.IdentityUserId equals credit.IdentityUserId
                        where !auction.IsSold
                        select new AuctionList
                        {
                            Id = auction.Id,
                            Title = auction.Title,
                            Description = auction.Description,
                            Price = auction.Price,
                            EndDate = auction.EndDate,
                            IsSold = auction.IsSold,
                            IdentityUserId = auction.IdentityUserId,
                            Username = auction.Username,
                            User = auction.User,
                            Bids = auction.Bids,
                            credits= currentcredit
                        };

        

            return query;
        }
       
        public async Task<AuctionList> GetById(int? id)
        {
            var listing = await _context.Listings
                .Include(l => l.Bids)
                .FirstOrDefaultAsync(m => m.Id == id);
            return listing;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

       
    }
}
