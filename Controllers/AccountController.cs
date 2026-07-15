using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KarisBrook.Models;
using System.Threading.Tasks;

namespace KarisBrook.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl ?? Url.Action("Index", "Home"));
                }

                // Добавляем общую ошибку, если вход не удался
                ModelState.AddModelError(string.Empty, "Неверный email или пароль.");
            }

            return View();
        }

        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
               
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Переводим ошибки на русский
                foreach (var error in result.Errors)
                {
                    var russianMessage = error.Code switch
                    {
                        "PasswordTooShort" => "Пароль должен содержать не менее 6 символов.",
                        "PasswordRequiresLower" => "Пароль должен содержать хотя бы одну строчную букву ('a'-'z').",
                        "PasswordRequiresUpper" => "Пароль должен содержать хотя бы одну заглавную букву ('A'-'Z').",
                        "PasswordRequiresDigit" => "Пароль должен содержать хотя бы одну цифру ('0'-'9').",
                        "PasswordRequiresNonAlphanumeric" => "Пароль должен содержать хотя бы один специальный символ.",
                        "DuplicateUserName" => "Пользователь с таким email уже зарегистрирован.",
                        "DuplicateEmail" => "Пользователь с таким email уже зарегистрирован.",
                        _ => error.Description // Если ошибка не из списка — оставляем как есть
                    };
                    ModelState.AddModelError(string.Empty, russianMessage);
                }
            }

            return View();
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Profile
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}