using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friello.DAL;
using Friello.Models;
using Microsoft.AspNetCore.Mvc;


namespace Friello.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/

        public IActionResult Index()
        {
            //List<Slider> sliders = _context.Sliders.ToList();

            return View();
        }
    }
}
