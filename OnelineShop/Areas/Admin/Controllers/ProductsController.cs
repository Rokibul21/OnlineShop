using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnelineShop.Data;
using OnelineShop.Models;

namespace OnelineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _he;

        public ProductsController(ApplicationDbContext context,IHostingEnvironment he)
        {
            _context = context;
            _he = he;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.ProductType).Include(p => p.SpacialTag);
            return View(await applicationDbContext.ToListAsync());
        }
        //Post For Index
        public IActionResult Index(decimal lowAmount,decimal largAmount)
        {
            var products = _context.Products
                .Include(c => c.ProductTypes)
                .Include(c => c.SpacialTag).Where(c => c.Price >= lowAmount && c.Price <= largAmount);
            return View(products);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.SpacialTag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "ProductType");
            ViewData["SpacilTag"] = new SelectList(_context.spacialTags, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _context.Products.FirstOrDefault(c => c.Name == products.Name);
                if(searchProduct != null)
                {
                    ViewBag.Message = "This product is already exist";
                    ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "ProductType");
                    ViewData["SpacilTag"] = new SelectList(_context.spacialTags, "Id", "Name");
                    return View(products);
                }
                if(image!=null)
                {
                    var path = Path.Combine(_he.WebRootPath+"/images",Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(path, FileMode.Create));
                    products.Image = "images/" + image.FileName;
                }
                if(image==null)
                {
                    products.Image = "images/no-image-found.jpg";
                }
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["Save"] = "Product has been save successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypes);
            ViewData["SpacilTag"] = new SelectList(_context.spacialTags, "Id", "Name", products.SpacilTag);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypes);
            ViewData["SpacilTag"] = new SelectList(_context.spacialTags, "Id", "Name", products.SpacilTag);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var path = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(path, FileMode.Create));
                    products.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "images/no-image-found.jpg";
                }
                _context.Update(products);
                await _context.SaveChangesAsync();
                TempData["Save"] = "Product has been Edit successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypes"] = new SelectList(_context.ProductTypes, "Id", "ProductType", products.ProductTypes);
            ViewData["SpacilTag"] = new SelectList(_context.spacialTags, "Id", "Name", products.SpacilTag);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.SpacialTag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            TempData["Save"] = "Product has been Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
