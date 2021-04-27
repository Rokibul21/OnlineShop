using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnelineShop.Areas.Admin.Model;
using OnelineShop.Data;

namespace OnelineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;

        public object UserRoleAssign { get; private set; }

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            var role = _roleManager.Roles.ToList();
            ViewBag.Roles = role;
            return View();
        }

        public async Task<IActionResult>Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var result =await _roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                TempData["Save"] = "Role Has Been Save Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.Name = role.Name;
            return View(role);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.Message = "This Role is Allready Exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role Has Been Update Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.Name = role.Name;
            return View(role);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConferm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role Has Been delete Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Assign()
        {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleViewModel roleUser)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);
            var IsChackRoleAssign = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (IsChackRoleAssign)
            {
                ViewBag.msg = "This User Already Assign this Role";
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }
            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);

            if (role.Succeeded)
            {
                TempData["Save"] = "User Role Has Been Successfuly Assign";
                return RedirectToAction(nameof(UserRoleAssigns));
            }
            return View();

        }
        public IActionResult UserRoleAssigns()
        {
            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join a in _db.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRole = result;
            return View();
        }
    }
}