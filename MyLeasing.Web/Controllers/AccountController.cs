using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return View();
            }

            ViewBag.UserMessage = TempData["UserMessage"];

            var model = new UserAccountViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Document = user.Document,
                Address = user.Address
            };

            return View(model);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Document = model.Document;
                    user.Address = model.Address;
                };

                var updateUser = await _userHelper.UpdateUserAsync(user);

                if (updateUser.Succeeded)
                {
                    TempData["UserMessage"] = "Sucessfully updated.";
                    return RedirectToAction(nameof(Index));
                }
            }

            ModelState.AddModelError(string.Empty, "Could not update user.");
            return View("Index");
        }


        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var changePassword = await _userHelper.ChangePasswordAsync(
                        user, model.OldPassword, model.NewPassword);

                    if (changePassword.Succeeded)
                    {
                        TempData["UserMessage"] = "Sucessfully updated password.";
                        return RedirectToAction(nameof(Index), model);
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Could not update password.");
            return View();
        }


        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToHomePage();
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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


        public async Task<IActionResult> Logout()
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


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
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
                    LastName = model.LastName,
                    Address = model.Address
                };

                var register = await _userHelper.AddUserAsync(user, model.Password);

                if (register.Succeeded)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Standard");

                    var loginViewModel = new LoginViewModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                        RememberMe = false
                    };
                    return await Login(loginViewModel);
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
