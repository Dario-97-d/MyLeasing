using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLeasing.Web.Data.Entities;
using MyLeasing.Web.Helpers;
using MyLeasing.Web.Models;

namespace MyLeasing.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }


        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Login();
            }

            return RedirectToHomePage();
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHomePage();
            }

            return View();
        }


        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHomePage();
            }

            if (ModelState.IsValid)
            {
                var signIn = await _userHelper.LoginAsync(model);

                if (signIn.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"]);
                    }
                    else
                    {
                        return RedirectToHomePage();
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not login.");
            return View(model);
        }


        [ActionName("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _userHelper.LogoutAsync();
            }

            return RedirectToHomePage();
        }


        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHomePage();
            }

            return View();
        }


        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterNewUserViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHomePage();
            }

            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "This e-mail is already registered.");
                    return View(model);
                }

                user = new User
                {
                    UserName = model.Username,
                    Email = model.Username,
                    Document = model.Document,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                
                var register = await _userHelper.AddUserAsync(user, model.Password);

                if (register.Succeeded)
                {
                    var loginViewModel = new LoginViewModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                        RememberMe = false
                    };
                    return await LoginAsync(loginViewModel);
                }
            }

            ModelState.AddModelError(string.Empty, "Could not register user.");
            return View(model);
        }


        public IActionResult RedirectToHomePage()
        {
            return RedirectToAction("Index", nameof(HomeController).Replace("Controller", null));
        }
    }
}
