using Friello.DAL;
using Friello.Extentions;
using Friello.Models;
using Friello.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page=1, int take=5)
        {
            List<Product> products = _context.Products.Include(p => p.CategoryName).Skip((page-1)*take).Take(take).ToList();
            PaginationVM<Product> paginationVM = new PaginationVM<Product>(products, PageCount(take), page);
            return View(products);

        }
        private int PageCount(int take)
        {
            List<Product> products = _context.Products.ToList();
            return (int)Math.Ceiling((decimal)products.Count() / take);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.Include(p=>p.CategoryName).FirstOrDefaultAsync(p=>p.Id==id);
            if (dbProduct == null) return NotFound();
            return View(dbProduct);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = ViewBag.Categories = _context.Categories.ToList();
            if (product.Photo==null)
            {
                ModelState.AddModelError("Photo", "bosh qalmasin");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "tolka sekil olsun");
                return View();
            }
            if (product.Photo.ValidSize(200))
            {
                ModelState.AddModelError("Photo", "oversized  ");
                return View();
            }
            
            Product newProduct = new Product
            {
                Price = product.Price,
                Name = product.Name,
                CategoryId = product.CategoryId,
                ImageUrl = product.Photo.SaveImage(_env, "img")
            };  

            
            await _context.Products.AddAsync(newProduct);
            _context.SaveChanges();
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", dbProduct.ImageUrl);
            Helpers.Helper.DeleteImage(path);
            _context.Products.Remove(dbProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "id", "Name");

            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();

            return View(dbProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "id", "Name");
            Product dbProduct = _context.Products.FirstOrDefault(c => c.Id == product.Id);

            if (product.Photo == null)
            {
                ModelState.AddModelError("Photo", "bosh qalmasin");
                return View();
            }
            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "tolka sekil olsun");
                return View();
            }
            if (product.Photo.ValidSize(200))
            {
                ModelState.AddModelError("Photo", "oversized");
                return View();
            }

            string path = Path.Combine(_env.WebRootPath, "img", dbProduct.ImageUrl);
            System.IO.File.Delete(path);

            dbProduct.Photo = product.Photo;
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ImageUrl = product.Photo.SaveImage(_env, "img");
            dbProduct.CategoryId = product.CategoryId;

            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
