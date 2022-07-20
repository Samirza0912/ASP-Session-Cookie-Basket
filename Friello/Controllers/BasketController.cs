using Friello.DAL;
using Friello.Models;
using Friello.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddItem(int? id)
        {
            if (id == null) return NotFound();
            Product dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null) return NotFound();
            List<BasketVM> products;
            if (Request.Cookies["basket"] == null)
            {
                products = new List<BasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            BasketVM existProduct = products.Find(x => x.Id == id);
            if (existProduct == null)
            {
                BasketVM basketVm = new BasketVM
                {
                    Id = dbProduct.Id,
                    ProductCount = 1
                };
                products.Add(basketVm);
            }
            else
            {
                existProduct.ProductCount++;
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(5) });
            //HttpContext.Session.SetString("name", "Samir");
            //Response.Cookies.Append("group", "p322", new CookieOptions { MaxAge = TimeSpan.FromDays(14) });

            return RedirectToAction("index", "home");
        }
        public IActionResult ShowItem()
        {
            //string name = HttpContext.Session.GetString("name");
            //string group = Request.Cookies["group"];
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            if (basket != null)
            {
                products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var item in products)
                {
                    Product dbProduct = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                    item.Price = dbProduct.Price;
                    item.ImageUrl = dbProduct.ImageUrl;
                    item.Name = dbProduct.Name;
                }
            }
            else
            {
                products = new List<BasketVM>();
            }
            return View(products);
        }
        public IActionResult Remove(int id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            BasketVM removeProduct = products.Find(p => p.Id == id);
            products.Remove(removeProduct);
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            return RedirectToAction("showItem", "basket");
        }
        public IActionResult Plus(int id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            BasketVM del = products.FirstOrDefault(x => x.Id == id);

            if (del.ProductCount <= 10)
            {
                del.ProductCount++;
            }
            else
            {
                NotFound();
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            return RedirectToAction("showitem", "basket");

        }
        public IActionResult Minus(int id)
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            BasketVM del = products.FirstOrDefault(x => x.Id == id);

            if (del.ProductCount > 1)
            {
                del.ProductCount--;
            }
            else
            {
                products.Remove(del);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(products), new CookieOptions { MaxAge = TimeSpan.FromDays(14) });


            return RedirectToAction("showitem", "basket");
        }
        [HttpPost]
        [ActionName("Basket")]
        public async Task<IActionResult> Sale()
        {
            if (!User.Identity.IsAuthenticated) return View("login", "account");
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                Sale sale = new Sale();
                sale.SaleDate = DateTime.Now;
                sale.AppUserId = user.Id;

                List<BasketVM> basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
                List<SalesProduct> salesProducts = new List<SalesProduct>();
                double total = 0;
                foreach (var basketProduct in basketProducts)
                {
                    Product dbProduct = await _context.Products.FindAsync(basketProduct.Id);
                    if (basketProduct.ProductCount>dbProduct.Count)
                    {
                        TempData["fail"] = "sale sehvdi";
                        return RedirectToAction("ShowItem");
                    }
                    
                    SalesProduct salesProduct = new SalesProduct();
                    salesProduct.ProductId = dbProduct.Id;
                    salesProduct.Count = basketProduct.ProductCount;
                    salesProduct.Price = dbProduct.Price;
                    salesProduct.SaleId = sale.Id;
                    salesProducts.Add(salesProduct);
                    total += basketProduct.ProductCount * dbProduct.Price;
                }
                sale.SalesProducts = salesProducts;
                sale.Total = total;
                await _context.AddAsync(sale);
                await _context.SaveChangesAsync();
                TempData["success"] = "sale duzdu";
                return RedirectToAction("ShowItem");
            }
        }
    }
}
