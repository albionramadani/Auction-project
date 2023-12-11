

using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data.Services
{
    public interface IUserCreditService
    {
        Task CreditForNewUser(string uca);
        Task TransferFunds(string fromUserId, string toUserId, double amount);
       
        double CreditById(string id);
    }
}
