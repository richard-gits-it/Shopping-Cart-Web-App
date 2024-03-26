using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.ProductMaster
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
        ViewData["CategoryId"] = new SelectList(_context.Set<Categories>(), "Id", "Name");

            return Page();
        }

        public List<SelectListItem> Categories { get; set; }


        [BindProperty]
        public Products Products { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (_context.Products == null || Products == null)
            {
                return Page();
            }

            _context.Products.Add(Products);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
