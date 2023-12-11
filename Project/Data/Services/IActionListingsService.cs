using Project.Models;

namespace Project.Data.Services
{
    public interface IActionListingsService
    {
        IQueryable<AuctionList> GetAll();
        IQueryable<AuctionList> GetAllAuctionsWithCredits(string currentUserId);
        Task Add(AuctionList auctionlisting);
        Task<AuctionList> GetById(int? id);
        Task SaveChanges();
         
    }
}
