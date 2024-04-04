using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace multiple_tables.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(UserManager<IdentityUser> userManager, multiple_tables.Data.ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<CartProducts> CartProducts { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page if not authenticated
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get user id
            var userId = _userManager.GetUserId(User);

            // Check if user id is not null
            if (userId != null)
            {
                // Retrieve the user's cart including associated products
                var userCart = await _context.Cart
                    .Include(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // If user's cart doesn't exist, create a new one
                if (userCart == null)
                {
                    var newCart = new Cart { UserId = userId };
                    _context.Cart.Add(newCart);
                    await _context.SaveChangesAsync();

                    userCart = newCart;
                }

                // Check if cart products exist
                if (userCart.CartProducts != null)
                {
                    CartProducts = userCart.CartProducts.ToList();
                }
                else
                {
                    // If cart products don't exist, initialize an empty list
                    CartProducts = new List<CartProducts>();
                }
            }

            // Return the page
            return Page();
        }

    }
}
