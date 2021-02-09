using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnelineShop.Data;
using OnelineShop.Models;

namespace OnelineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpacialTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpacialTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SpacialTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.spacialTags.ToListAsync());
        }

        // GET: Admin/SpacialTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spacialTag = await _context.spacialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spacialTag == null)
            {
                return NotFound();
            }

            return View(spacialTag);
        }

        // GET: Admin/SpacialTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SpacialTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SpacialTag spacialTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spacialTag);
                await _context.SaveChangesAsync();
                TempData["Save"] = "Spacial Tag has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(spacialTag);
        }

        // GET: Admin/SpacialTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spacialTag = await _context.spacialTags.FindAsync(id);
            if (spacialTag == null)
            {
                return NotFound();
            }
            return View(spacialTag);
        }

        // POST: Admin/SpacialTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SpacialTag spacialTag)
        {
            if (id != spacialTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spacialTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpacialTagExists(spacialTag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Save"] = "Spacial Tag has been Edit successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(spacialTag);
        }

        // GET: Admin/SpacialTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spacialTag = await _context.spacialTags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spacialTag == null)
            {
                return NotFound();
            }

            return View(spacialTag);
        }

        // POST: Admin/SpacialTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spacialTag = await _context.spacialTags.FindAsync(id);
            _context.spacialTags.Remove(spacialTag);
            await _context.SaveChangesAsync();
            TempData["Save"] = "Spacial Tag has been Deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool SpacialTagExists(int id)
        {
            return _context.spacialTags.Any(e => e.Id == id);
        }
    }
}
