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

namespace multiple_tables.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Cart> Cart { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);

            // Check if user exists
            if (currentUser != null)
            {
                // Get the cart for the current user
                Cart = await _context.Cart
                    .Where(c => c.UserId == currentUser.Id)
                    .Include(c => c.User)
                    .ToListAsync();
            }
        }
    }
}
