using Microsoft.AspNetCore.Mvc;

namespace multiple_tables.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult AddToCart(int productId)
        {
            return RedirectToAction("Index");
        }
    }
}
