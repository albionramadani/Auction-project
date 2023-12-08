using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Diagnostics;

namespace Project.Controllers
{
   
    public class HomeController : Controller
    {
        //private readonly ApplicationDbContext _context;
        //public ApplicationDbContext db = new ApplicationDbContext();
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;


        //public HomeController(
        //    ILogger<HomeController> logger,
        //    ApplicationDbContext context,
        //    UserManager<User> userManager,
        //    SignInManager<User> signInManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _context = context;
        //}





        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}