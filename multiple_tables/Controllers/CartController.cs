using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using multiple_tables.models;
using System.Text.Json;

namespace multiple_tables.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int productId)
        {
            //check if there is user signed in
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                //get user id
                var userId = _httpContextAccessor.HttpContext.User.Identity.Name;

                //get Cart from db
                Cart cart = new Cart();

                if (_httpContextAccessor.HttpContext.Session.TryGetValue("Cart", out byte[] cartData))
                {
                    cart = JsonSerializer.Deserialize<Cart>(cartData);
                }

                //check if product is already in Cart
                var product = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);

                if (product != null)
                {
                    product.Quantity++;//adds one to quantity
                }
                else
                {
                    cart.CartProducts.Add(new CartProducts { ProductId = productId, Quantity = 1 });//adds product to Cart
                }

                _httpContextAccessor.HttpContext.Session.Set("Cart", JsonSerializer.SerializeToUtf8Bytes(cart));
            }
            else
            {
                //if user is not signed in, use session to store Cart
                List<CartProductsSession> Cart;
                if (_httpContextAccessor.HttpContext.Session.TryGetValue("Cart", out byte[] cartData))
                {
                    Cart = JsonSerializer.Deserialize<List<CartProductsSession>>(cartData);
                }
                else
                {
                    Cart = new List<CartProductsSession>();
                }
              
                //check if product is already in Cart
                var product = Cart.FirstOrDefault(cp => cp.ProductId == productId);

                if (product != null)
                {
                    product.Quantity++;//adds one to quantity
                }
                else
                {
                    Cart.Add(new CartProductsSession { ProductId = productId, Quantity = 1 });//adds product to Cart
                }

                _httpContextAccessor.HttpContext.Session.Set("Cart", JsonSerializer.SerializeToUtf8Bytes(Cart));
            }

            return RedirectToAction("Index");
        }
    }
}
