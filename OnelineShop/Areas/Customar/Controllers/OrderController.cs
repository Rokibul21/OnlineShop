using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnelineShop.Data;
using OnelineShop.Models;
using OnelineShop.Utility;

namespace OnelineShop.Areas.Customar.Controllers
{
    [Area("Customar")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(Order order)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products!=null)
            {
                foreach(var product in products)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo = GetOrederNo();
            _context.orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Set("products", null);
            return View();
        }

        public string GetOrederNo()
        {
            int rowCount = _context.orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}