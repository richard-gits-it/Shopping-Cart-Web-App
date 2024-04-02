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

      public CartProducts CartProducts { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CartProducts == null)
            {
                return NotFound();
            }

            var cartproducts = await _context.CartProducts.FirstOrDefaultAsync(m => m.CartId == id);
            if (cartproducts == null)
            {
                return NotFound();
            }
            else 
            {
                CartProducts = cartproducts;
            }
            return Page();
        }
    }
}
