using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace multiple_tables.Pages.ShoppingCart
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string orderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string customerName { get; set; }
        public void OnGet()
        {

        }
    }
}
