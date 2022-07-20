using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friello.DAL;
using Friello.Models;
using Friello.Services;
using Friello.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friello.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISum _sum;
        public HomeController(AppDbContext context, ISum sum)
        {
            _context = context;
            _sum = sum;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Sliders = _context.Sliders.ToList();
            homeVM.SliderContents = _context.SliderContents.FirstOrDefault();
            homeVM.Categories = _context.Categories.ToList();
            homeVM.Products = _context.Products.Include(p => p.CategoryName).ToList();
            int result = _sum.Sum(11, 12);

            return View(homeVM);
        }

        public IActionResult SearchProduct(string search)
        {
            List<Product> products = _context.Products
                 .Include(p => p.CategoryName)
                 .OrderBy(p => p.Id)
                 .Where(p => p.Name.ToLower()
                 .Contains(search.ToLower()))
                 .Take(5).ToList();

            return PartialView("_SearchPartial", products);
        }
    }
}

