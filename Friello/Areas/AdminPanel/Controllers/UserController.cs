using Friello.Models;
using Friello.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Index(string search)
        {
            var users = search == null ?
                _userManager.Users.ToList() : 
                _userManager.Users
                .Where(u => u.Fullname.ToLower().Contains(search.ToLower())).ToList();
            return View(users);
        }
        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var userRoles = _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            RoleVM roleVM = new RoleVM
            {
                Fullname = user.Fullname,
                roles = roles,
                //userRoles = userRoles,
                userId=user.Id
            };
            return View(roleVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(List<string> roles, string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            var userRoles = _userManager.GetRolesAsync(user);

            //var addRoles = roles.Except(userRoles);
            //var removedRoles = userRoles.Except(roles);
            //await _userManager.AddToRoleAsync(user, addRoles);
            //await _userManager.RemoveFromRolesAsync(user, removedRoles);
            return RedirectToAction("index");
        }
    }
}
