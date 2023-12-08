using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace NewsApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("register")]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = userModel.Username,
                    LastName = userModel.Username,
                    UserName = userModel.Username,
                    Email = userModel.Email
                };
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
             
            return View(userModel);

        }

        [Route("signin")]
        public IActionResult SignIn()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (signInResult.Succeeded)
                        return RedirectToAction("Index", "Home");
                    else
                    {
                        ModelState.AddModelError("", "Incorrect login details");
                        return View(model);
                    }
                }
                ModelState.AddModelError("", "An account with this email doesn't exist!");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()   
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }
    }
}