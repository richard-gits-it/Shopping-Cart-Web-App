using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.OrderMaster
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Orders> Orders { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Orders = await _context.Orders.ToListAsync();
            }
        }
        public IActionResult OnPostDetails(int id)
        {
            return RedirectToPage("/OrderHistory/Details", new { orderId = id });
        }
    }
}
