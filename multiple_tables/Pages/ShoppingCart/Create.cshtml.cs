using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.ShoppingCart
{
    public class CreateModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public CreateModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id");
        ViewData["ProductId"] = new SelectList(_context.Products, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public CartProducts CartProducts { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (_context.CartProducts == null || CartProducts == null)
            {
                return Page();
            }

            _context.CartProducts.Add(CartProducts);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
