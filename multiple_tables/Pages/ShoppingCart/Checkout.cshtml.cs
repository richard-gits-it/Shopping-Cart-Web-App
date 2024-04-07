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
            //initialize order
            Order = new Orders();
        }

        public IList<CartProducts> CartProducts { get; set; } = default!;
        public double MerchandiseTotal { get; set; }
        public double ShippingCost { get; set; } = 5;
        //property for gst calculation 
        public double FederalTaxGST => (MerchandiseTotal + ShippingCost) * 0.05;
        public double TotalPrice => MerchandiseTotal + ShippingCost + FederalTaxGST;


        [BindProperty]
        public Orders Order { get; set; } = default!;

        [BindProperty]
        public Payment Payment { get; set; }


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

                // Populate CustomerId
                Order.CustomerId = userId;

                // Load the order details entities
                Order.OrderDetails = CartProducts.Select(cp => new OrderDetails
                {
                    Product = cp.Product,
                    UnitPrice = cp.Product.UnitPrice,
                    Quantity = cp.Quantity,
                    Discount = 0
                }).ToList();

                // Store Order object in session as a serialized JSON string
                _httpContextAccessor.HttpContext.Session.SetString("Order", System.Text.Json.JsonSerializer.Serialize(Order));

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

        public IActionResult OnPost()
        {

            //if (!ModelState.IsValid)
            //{
            //    //repopulate CartProducts if ModelState is not valid
            //    var _userId = _userManager.GetUserId(User);
            //    if (_userId != null)
            //    {
            //        var userCart = _context.Cart
            //            .Include(c => c.CartProducts)
            //                .ThenInclude(cp => cp.Product)
            //            .FirstOrDefault(c => c.UserId == _userId);

            //        if (userCart != null && userCart.CartProducts != null)
            //        {
            //            CartProducts = userCart.CartProducts.ToList();
            //            MerchandiseTotal = CartProducts.Sum(cp => cp.TotalPrice);
            //        }
            //        else
            //        {
            //            // Handle the case where the user's cart or cart products are null
            //            // Redirect or handle it according to your application's logic
            //            return RedirectToPage("/ShoppingCart/Index");
            //        }
            //    }
            //    else
            //    {
            //        // Handle the case where userId is null
            //        // Redirect or handle it according to your application's logic
            //        return RedirectToPage("/Account/Login", new { area = "Identity" });
            //    }

            //    // Return the page with the updated ModelState
            //    return Page();
            //}

            // Get user id
            var userId = _userManager.GetUserId(User);

            // Check if user id is not null
            if (userId != null)
            {
                // Retrieve the user's cart including associated products
                var userCart = _context.Cart
                    .Include(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                    .FirstOrDefault(c => c.UserId == userId);

                // If user's cart doesn't exist, create a new one
                if (userCart == null)
                {
                    var newCart = new Cart { UserId = userId };
                    _context.Cart.Add(newCart);
                    _context.SaveChanges();

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

                //save order details
                var order = new Orders
                {
                    CustomerId = userId,
                    OrderDate = DateTime.Now,
                    RequiredDate = DateTime.Now.AddDays(7),
                    ShippedDate = DateTime.Now.AddDays(2),
                    FirstName = Order.FirstName,
                    LastName = Order.LastName,
                    ShipAddress = Order.ShipAddress,
                    ShipCity = Order.ShipCity,
                    ShipProvince = Order.ShipProvince,
                    ShipPostalCode = Order.ShipPostalCode,
                    ShipCountry = Order.ShipCountry,
                    ShipPhone = Order.ShipPhone,
                    ShipEmail = Order.ShipEmail
                };
                //add order to the database
                _context.Orders.Add(order);
                _context.SaveChanges();

                //add order details to the database
                foreach (var cartProduct in CartProducts)
                {
                    var orderDetails = new OrderDetails
                    {
                        Order = order,
                        Product = cartProduct.Product,
                        UnitPrice = cartProduct.Product.UnitPrice,
                        Quantity = cartProduct.Quantity,
                        Discount = 0
                    };
                    _context.OrderDetails.Add(orderDetails);
                }
                _context.SaveChanges();

                // Clear cart
                _context.CartProducts.RemoveRange(CartProducts);
                _context.SaveChanges();

                // Redirect to the order confirmation page with the order id
                return RedirectToPage("/ShoppingCart/OrderConfirmation", new { orderId = order.Id, customerName = order.FirstName + " " + order.LastName });
            }

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

    }
}
