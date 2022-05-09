#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBook.Data;
using FPTBook.Models;
using Microsoft.AspNetCore.Identity;
using FPTBook.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace FPTBook.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FPTBookContext _context;
        private readonly UserManager<FPTBookUser> _userManager;
        public OrdersController(FPTBookContext context, UserManager<FPTBookUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string thisUserId = _userManager.GetUserId(HttpContext.User);
            var userContext = _context.Order.Where(o => o.UId == thisUserId).Include(o => o.User);
            return View(await userContext.ToListAsync());
        }
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> orderManage( string searchString)
        {
            var customer = from c in _userManager.Users
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                customer = customer.Where(c => c.Email!.Contains(searchString));
            }
            var userContext = _context.Order.Include(o => o.User);
            return View(await userContext.ToListAsync());
        }

    }
}
