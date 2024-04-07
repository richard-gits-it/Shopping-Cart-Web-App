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

namespace multiple_tables.Pages.ProductMaster
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public DetailsModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Products Products { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }
            else 
            {
                Products = products;
            }
            return Page();
        }
    }
}
