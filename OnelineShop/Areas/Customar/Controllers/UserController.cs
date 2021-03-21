﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnelineShop.Data;
using OnelineShop.Models;

namespace OnelineShop.Areas.Customar.Controllers
{
    
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        public UserController(UserManager<IdentityUser> userManager,ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [Area("Customar")]
        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }
        [Area("Customar")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Area("Customar")]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    TempData["save"] = "User Has Been Save Successfuly";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        //public IActionResult Detials(string id)

        //{
        //    if(id==null)
        //    {
        //        return NotFound();
        //    }
        //    var user =_context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
        //    if(user==null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}
        [Area("Customar")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c=>c.Id==id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [Area("Customar")]
        public async Task<IActionResult> Edit(ApplicationUser User)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == User.Id);
            if(userInfo==null)
            {
                return NotFound();
            }
            userInfo.FristName = User.FristName;
            userInfo.LastName = User.LastName;
            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["save"] = "User Has Been Save Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }
    }
}