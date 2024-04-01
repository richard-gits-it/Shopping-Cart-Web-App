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

namespace multiple_tables.Pages.ProductMaster
{
    public class EditModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public EditModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Products Products { get; set; } = default!;

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var products =  await _context.Products.FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }
            Products = products;
           ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (_context.Products == null || Products == null || ImageFile == null)
            //{
            //    return Page();
            //}

            _context.Attach(Products).State = EntityState.Modified;

            if (ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageFile.CopyToAsync(memoryStream);
                    Products.ImageData = memoryStream.ToArray();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(Products.ID))
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

        private bool ProductsExists(int id)
        {
          return (_context.Products?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
