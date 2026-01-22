using Exam.Models;
using Exam.Utilities.Enums;
using Exam.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Exam.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = new()
            {
                Name = registerVM.Name,
                SurName = registerVM.SurName,
                UserName = registerVM.Username,
                Email = registerVM.Email,

            };

            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString()); //Admin hesabi username: admin // password: Admin123!
            // user hesabi username : rzayevkenan // pass: Kanan123!

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnURL)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginVM.UsernameorEmail || u.UserName == loginVM.UsernameorEmail);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Username,Email or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersistant, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is blocked");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username ,Email or password is incorrect");
                return View();
            }

            if (returnURL is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return Redirect(returnURL);
        }

        public async Task<IActionResult> Logout(string? returnURL)
        {
            await _signInManager.SignOutAsync();

            if (returnURL is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return Redirect(returnURL);
        }

        //rollari yaratmaq
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}
    }
}
