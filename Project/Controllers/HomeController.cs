using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data.Services;
using Project.Models;
using System.Diagnostics;
using System.Reflection;
using System.Security.Claims;
using Project.Models;
using Hangfire;
using System.Security.Cryptography;

namespace Project.Controllers
{
   
    public class HomeController : Controller
    {
      
        private readonly IBidsService _bidsService;
        private readonly IActionListingsService _listingsService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserCreditService _userCreditService;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _backgroundJobClient;
       
        public HomeController(IBackgroundJobClient backgroundJobClient,ApplicationDbContext context, IUserCreditService userCreditService, IActionListingsService listingsService, IWebHostEnvironment webHostEnvironment, IBidsService bidsService, ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _listingsService = listingsService ?? throw new ArgumentNullException(nameof(listingsService));
            _userCreditService = userCreditService;
            _webHostEnvironment = webHostEnvironment;
            _bidsService = bidsService;
            _backgroundJobClient = backgroundJobClient;
            _userManager = userManager;
            _logger = logger;
            _context = context;

        }

        // GET: Listings
        public async Task<IActionResult> Index(int? pageNumber)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = _userManager.GetUserId(User);
                    var applicationDbContext = _listingsService.GetAllAuctionsWithCredits(currentUserId);
                    int pageSize = 10;
                    var auctionsToMarkAsSold = applicationDbContext.Where(l => l.IsSold == false && DateTime.Now > l.EndDate).ToList();
                    var credits = _userCreditService.CreditById(currentUserId);

                    foreach (var auction in auctionsToMarkAsSold)
                    {
                        auction.IsSold = true;
                        _context.Update(auction);
                        _context.SaveChanges();
                    }

                    foreach (var auction in auctionsToMarkAsSold)
                    {
                        if (auction.Bids.Count == 0)
                        {
                            auction.IsSold = true;
                        }
                        else
                        {
                            _backgroundJobClient.Enqueue(() => MarkAuctionAsSold(auction.Id, auction.Bids.FirstOrDefault().Price, currentUserId));
                        }
                    }
                    return View(await PaginatedList<AuctionList>.CreateAsync(applicationDbContext
                        .Where(l => l.IsSold == false)
                        .AsNoTracking(), pageNumber ?? 1, pageSize));
                }

                return RedirectToAction("SignIn", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on test controller, Index");
                return null;
            }
        }

        public async Task MarkAuctionAsSold(int auctionId, double price, string currentUserId)
        {
            var auction = _context.Listings.Find(auctionId);
            await _bidsService.CreditsToOwner(price, auction.IdentityUserId);
            await _bidsService.RemoveWallet(price, currentUserId);
            if (auction != null && !auction.IsSold && DateTime.Now > auction.EndDate)
            {
                auction.IsSold = true;
                _context.Update(auction);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var listing = await _listingsService.GetById(id);

                    if (listing == null)
                    {
                        return NotFound();
                    }

                    return View(listing);
                }
                return RedirectToAction("SignIn", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on test controller, Details");
                return null;
            }
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (id != null && id != 0)
                    {
                        var listing = await _context.Listings
                            .FirstOrDefaultAsync(m => m.Id == id);

                        if (listing != null)
                        {
                            var bids = _context.Bids
                                .Where(x => x.ListingId == id)
                                .ToList();
                            _context.Bids.RemoveRange(bids);
                            _context.Listings.Remove(listing);
                            await _context.SaveChangesAsync();
                        }

                    }
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("SignIn", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on test controller, Delete");
                return null;
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionViewModel listing)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        var currentDate = DateTime.Now;
                        var user = await _userManager.FindByIdAsync(listing.IdentityUserId);
                        if (listing.EndDate <= currentDate)
                        {
                            ModelState.AddModelError(nameof(listing.EndDate), "End date must be in the future.");
                            return View("Create", listing);
                        }
                        if (user != null)
                        {
                            var username = user.UserName;
                            var listObj = new AuctionList
                            {
                                Title = listing.Title,
                                Description = listing.Description,
                                Price = listing.Price,
                                IdentityUserId = listing.IdentityUserId,
                                Username = username,
                                EndDate = listing.EndDate,
                                User = user,
                            };
                            await _listingsService.Add(listObj);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "User not found");
                        }
                    }

                    return View(listing);
                }
                return RedirectToAction("SignIn", "Account");
            }
            
           
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on test controller, Create");
                return null;
            }
        }
            [HttpPost]
            public async Task<ActionResult> AddBid([Bind("Id, Price, ListingId, IdentityUserId")] Bid bid)
            {
                try
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var listing = await _listingsService.GetById(bid.ListingId);
                        var user = await _userManager.FindByIdAsync(bid.IdentityUserId);
                        var price = _userCreditService.CreditById(user.Id);
                        string currentUserId = _userManager.GetUserId(User);
                        if (listing.IdentityUserId == currentUserId)
                        {
                            ModelState.AddModelError(nameof(bid.Price), "You cannot to place your bid.");
                            return View("Details", listing);
                        }
                        if (ModelState.IsValid)
                        {
                            if (bid.Price > price)
                            {
                                ModelState.AddModelError(nameof(bid.Price), "You must have more credit to place this bid.");
                                return View("Details", listing);
                            }
                            await _bidsService.Add(bid);
                            listing.Price = bid.Price;
                            listing.User = bid.User;
                            await _listingsService.SaveChanges();
                            return View("Details", listing);
                        }
                    }
                    return RedirectToAction("SignIn", "Account");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on test controller, AddBid");
                    return null;
                 }
           
            }
    }
}