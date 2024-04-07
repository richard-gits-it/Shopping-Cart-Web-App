using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.OrderHistory
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public IndexModel(UserManager<IdentityUser> userManager, multiple_tables.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IList<Orders> Orders { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //if (_context.Orders != null)
            //{
            //    Orders = await _context.Orders.ToListAsync();
            //}

            var userId = _userManager.GetUserId(User);

            //get the orders for the logged in user
            Orders = await _context.Orders
                .Where(o => o.CustomerId == userId)
                .Include(o => o.OrderDetails)
                .ToListAsync();

        }

        public IActionResult OnPostDetails(int id)
        {
            return RedirectToPage("./Details", new { orderId = id });
        }
    }
}
