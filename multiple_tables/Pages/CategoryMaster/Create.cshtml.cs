using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.CategoryMaster
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public CreateModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Categories Categories { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() 
        {
          if (Categories == null)
            {
                return Page();
            }

            _context.Categories.Add(Categories);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
