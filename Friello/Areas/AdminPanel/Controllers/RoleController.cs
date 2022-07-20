using Friello.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public RoleController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = role });
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(result);
            return RedirectToAction("index");
        }
    }
}
