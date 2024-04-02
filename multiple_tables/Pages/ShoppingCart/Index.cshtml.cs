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
            //gets user id
            var userId = _userManager.GetUserId(User);

            if (userId != null)
            {
                var userCart = await _context.Cart
                    .Include(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null)
                {
                    var newCart = new Cart { UserId = userId };
                    _context.Cart.Add(newCart);
                    await _context.SaveChangesAsync();

                    userCart = newCart;
                }

                if (userCart.CartProducts != null)
                {
                    CartProducts = userCart.CartProducts.ToList();
                }
                else
                {
                    CartProducts = new List<CartProducts>();
                }
            }
            else //if user is not logged in, use session to store cart
            {
                Cart cart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("Cart", out byte[] cartData))
                {
                    cart = JsonSerializer.Deserialize<Cart>(cartData);
                }
                else
                {
                    cart = new Cart();
                    _httpContextAccessor.HttpContext.Session.Set("Cart", JsonSerializer.SerializeToUtf8Bytes(cart));
                }

                CartProducts = cart.CartProducts?.ToList() ?? new List<CartProducts>();
            }

            return Page();
        }

    }
}
