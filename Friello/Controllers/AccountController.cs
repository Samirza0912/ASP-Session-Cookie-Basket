using Friello.Models;
using Friello.ViewComponents;
using Friello.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Friello.Helpers.Helper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Friello.Areas.AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                Fullname = registerVM.Fullname,
                UserName = registerVM.Username,
                Email = registerVM.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
                await _signInManager.SignInAsync(user, true);
                await _userManager.AddToRoleAsync(user, UserRoles.Member.ToString());
            };
            return RedirectToAction("index", "home");
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("index", "home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "error");
                return View(loginVM);
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "error");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "error");
                return View(loginVM);
            }
            await _signInManager.SignInAsync(appUser, true);
            var roles = await _userManager.GetRolesAsync(appUser);
            foreach (var item in roles)
            {
                if (item=="Admin")
                {
                    return RedirectToAction("index", "dashboard", new { area = "AdminPanel" });
                }
            }
            if (ReturnUrl!=null)
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
        public async Task CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                }
            };
        }
    }
}
