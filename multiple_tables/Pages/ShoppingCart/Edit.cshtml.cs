using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.ShoppingCart
{
    public class EditModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public EditModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public OrderDetails OrderDetails { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderdetails =  await _context.OrderDetails.FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderdetails == null)
            {
                return NotFound();
            }
            OrderDetails = orderdetails;
           ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
           ViewData["ProductId"] = new SelectList(_context.Products, "ID", "ID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(OrderDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailsExists(OrderDetails.OrderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OrderDetailsExists(int id)
        {
          return (_context.OrderDetails?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
