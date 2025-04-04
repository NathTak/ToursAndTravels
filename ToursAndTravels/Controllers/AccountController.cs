using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ToursAndTravels.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // *********** LOGIN ***********
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userId, string password)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "User ID and Password are required!";
                return View();
            }

            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "User not found!";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Message = "Invalid credentials!";
            return View();
        }

        // *********** REGISTER ***********
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userId, string email, string password)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "All fields are required!");
                return View();
            }

            var user = new IdentityUser { UserName = userId, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // ✅ Ensure user is logged out before redirecting
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account"); // ✅ Redirect to Login Page
            }

            // ✅ Show errors properly
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }


        // *********** LOGOUT ***********
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // *********** CHANGE PASSWORD ***********
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Message = "All fields are required!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "New password and confirm password do not match!";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");  // ✅ Ensure correct route
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");  // ✅ Ensure correct route
            }

            ViewBag.Message = "Password change failed!";
            return View();
        }


        // *********** FORGOT PASSWORD ***********
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Email is required!";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Message = "User not found!";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            // TODO: Send resetLink via Email
            ViewBag.Message = "Password reset link has been sent to your email.";
            return View();
        }

        // *********** RESET PASSWORD ***********
        public IActionResult ResetPassword(string token, string email)
        {
            ViewBag.Token = token;
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Message = "All fields are required!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewBag.Message = "New password and confirm password do not match!";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Message = "User not found!";
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Message = "Password reset failed!";
            return View();
        }


    }
}
