using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneTraineeManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ValidateUserAsync(model.Username, model.Password);
                if (user != null)
                {
                    // Store user info in the session to track their logged-in state
                    HttpContext.Session.SetString("Username", user.Username);
                    HttpContext.Session.SetInt32("UserId", user.UserId);

                    // Redirect to the Dashboard after successful login
                    return RedirectToAction("Dashboard", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear the session to log the user out
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}