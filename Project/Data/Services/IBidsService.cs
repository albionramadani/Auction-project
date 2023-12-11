using Project.Models;

namespace Project.Data.Services
{
    public interface IBidsService
    {
        Task Add(Bid bid);
        Task RemoveWallet(double price, string user );
        Task CreditsToOwner(double price, string user );
        IQueryable<Bid> GetAll();
    }
}
