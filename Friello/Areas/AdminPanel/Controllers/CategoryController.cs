using Friello.DAL;
using Friello.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool existNameCategory = _context.Categories.Any(c => c.Name.ToLower() == category.Name.ToLower());
            if (existNameCategory)
            {
                ModelState.AddModelError("Name", "Does not exist");
            }
            Category newCategory = new Category
            {
                Name = category.Name,
                Desc = category.Desc,
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return NotFound();
            return View(dbCategory);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return NotFound();
            return View(dbCategory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category dbCategory = _context.Categories.FirstOrDefault(c => c.id == category.id);
            Category dbCategoryName = _context.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (dbCategoryName==null)
            {
                if (dbCategory.Name!=dbCategoryName.Name)
                {
                    ModelState.AddModelError("Name", "This name exists!!!");
                    return View();
                }
            }
            dbCategory.Name = category.Name;
            dbCategory.Desc = category.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Category dbCategory = await _context.Categories.FindAsync(id);
            if (dbCategory == null) return NotFound();
            _context.Categories.Remove(dbCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction();
        }

    }
}

