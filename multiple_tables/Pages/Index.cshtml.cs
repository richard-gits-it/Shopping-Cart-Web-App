using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using System.Drawing;
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
            //check if there is user signed in
            if (User.Identity.IsAuthenticated)
            {
                //get user id
                var userId = User.Identity.Name;

                //get Cart from db
                Cart cart = await _context.Cart
                    .Include(c => c.CartProducts)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null)
                {
                    cart = new Cart { UserId = userId };
                    _context.Cart.Add(cart);
                    await _context.SaveChangesAsync();
                
                }
                //check if product is already in Cart
                var product = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);



            }
            else
            {
                //if user is not signed in, redirect to login page
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            return RedirectToPage("/Index");
        }
    }
}