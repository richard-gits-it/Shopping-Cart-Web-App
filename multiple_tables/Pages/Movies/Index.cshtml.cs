using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public IndexModel(multiple_tables.Data.ApplicationDbContext context, 
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IList<Products> Product { get;set; } = default!;
        public IList<Customers> Customer { get; set; } = default!;
        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products.ToListAsync();
            }
            if (_context.Customer != null)
            {
                Customer = await _context.Customer.ToListAsync();
            }
            var user = await _userManager.GetUserAsync(User);
            List<IdentityUser> users = _userManager.Users.ToList();

            if (user != null)
            {
                string email = user.Email;
            }

        }
    }
}
