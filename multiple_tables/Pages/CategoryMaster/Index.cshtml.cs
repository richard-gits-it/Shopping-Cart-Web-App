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
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Categories> Categories { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Categories != null)
            {
                Categories = await _context.Categories.ToListAsync();
            }
        }
    }
}
