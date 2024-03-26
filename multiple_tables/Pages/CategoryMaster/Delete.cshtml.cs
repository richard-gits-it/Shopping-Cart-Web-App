using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.CategoryMaster
{
    public class DeleteModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public DeleteModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Categories Categories { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (categories == null)
            {
                return NotFound();
            }
            else 
            {
                Categories = categories;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var categories = await _context.Categories.FindAsync(id);

            if (categories != null)
            {
                Categories = categories;
                _context.Categories.Remove(Categories);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
