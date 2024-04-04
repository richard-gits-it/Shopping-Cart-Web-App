using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using System.Drawing;
using System.Security.Claims;
using System.Text.Json;

namespace multiple_tables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Products> Products { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Products = await _context.Products
                .Include(p => p.Category).ToListAsync();
            }
        }

        public byte[] ImageToByteArray(Image image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); // Change format if needed
                return memoryStream.ToArray();
            }
        }

        //use cartcontroller to add to cart
        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            // Check if there is a user signed in
            if (User.Identity.IsAuthenticated)
            {
                // Get user id from identity
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                // Get Cart from db
                Cart cart = await _context.Cart
                    .Include(c => c.CartProducts)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // If cart doesn't exist, create a new one
                if (cart == null)
                {
                    cart = new Cart { UserId = userId };
                    _context.Cart.Add(cart);
                }

                // Check if the product already exists in the cart
                var existingProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);
                if (existingProduct != null)
                {
                    // If the product already exists, increase its quantity
                    existingProduct.Quantity++;
                }
                else
                {
                    // If the product doesn't exist, add it to the cart
                    cart.CartProducts.Add(new CartProducts { ProductId = productId, Quantity = 1 });
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                // If user is not signed in, redirect to login page
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Redirect to index page after adding the product to the cart
            return RedirectToPage("/Index");
        }
    }
}