using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using multiple_tables.Services;

namespace multiple_tables.Pages.ProductMaster
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private DataLayer dataLayer = new DataLayer();

        public IndexModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;

        }

        public IList<Products> Products { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {

            if (_context.Products != null)
            {
                Products = await _context.Products
                .Include(p => p.Category).ToListAsync();
            }
        }


    }
}
