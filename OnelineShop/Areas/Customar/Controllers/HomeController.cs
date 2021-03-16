using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnelineShop.Data;
using OnelineShop.Models;
using OnelineShop.Utility;
using X.PagedList;

namespace OnelineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Area("Customar")]
        public IActionResult Index(int? page)
        {
            return View(_context.Products.Include(p => p.ProductType).Include(p => p.SpacialTag).ToList().ToPagedList(page??1,9));
        }
        [Area("Customar")]
        public IActionResult Detail( int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if(product==null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Area("Customar")]
        [HttpPost]
        [ActionName("Detail")]
        public IActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products==null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }
        [Area("Customar")]
        [ActionName("Remove")]
        public IActionResult RemovetoCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Area("Customar")]
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products= HttpContext.Session.Get<List<Products>>("products");
            if(products!=null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if(product!=null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [Area("Customar")]
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products==null)
            {
                products = new List<Products>();
            }
            return View(products); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
