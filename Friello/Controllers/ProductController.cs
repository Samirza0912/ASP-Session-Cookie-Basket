using Friello.DAL;
using Friello.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //List<Product> products = _context.Products.Take(8).ToList();

            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id==null) 
            {
                return NotFound();
            }
            Product dbProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }
    }
}
