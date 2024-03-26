using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.ShoppingCart
{
    public class DetailsModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public DetailsModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public OrderDetails OrderDetails { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderdetails = await _context.OrderDetails.FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderdetails == null)
            {
                return NotFound();
            }
            else 
            {
                OrderDetails = orderdetails;
            }
            return Page();
        }
    }
}
