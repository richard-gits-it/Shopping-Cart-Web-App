using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CheckoutModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutModel(UserManager<IdentityUser> userManager, multiple_tables.Data.ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<CartProducts> CartProducts { get; set; } = default!;
        public double MerchandiseTotal { get; set; }
        public double ShippingCost { get; set; } = 5;
        //property for gst calculation 
        public double FederalTaxGST => (MerchandiseTotal + ShippingCost) * 0.05;
        public double TotalPrice => MerchandiseTotal + ShippingCost + FederalTaxGST;

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

                    // Calculate total price of the cart
                    MerchandiseTotal = CartProducts.Sum(cp => cp.TotalPrice);
                }
                else
                {
                    // Redirect to the shopping cart page if no cart products exist
                    return RedirectToPage("/ShoppingCart/Index");
                }
            }

            // Return the page
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCartItemAsync(int cartId, int productId)
        {
            var cartProduct = await _context.CartProducts.FindAsync(cartId, productId);

            if (cartProduct != null)
            {
                _context.CartProducts.Remove(cartProduct);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/ShoppingCart/Index");

        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            throw new NotImplementedException();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Province is required")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\+(?:[0-9] ?){6,14}[0-9]$", ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}
