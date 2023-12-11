using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Project.Models
{
    public class ApplicationDbContext : IdentityDbContext< User>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<AuctionList> Listings { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<UserCreditAmmount> UserCredit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=(local)\\sqlexpress;Database=Pragmatic5;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connectionString, builder => builder.EnableRetryOnFailure());
        }
    }
}
