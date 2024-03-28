using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using Newtonsoft.Json;

namespace multiple_tables.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<OrderDetails> OrderDetails { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // Check if the cookie exists
            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("OrderDetailsCookie"))
            {
                // Retrieve the cookie value
                var orderDetailsJson = _httpContextAccessor.HttpContext.Request.Cookies["OrderDetailsCookie"];

                // Deserialize JSON string to a list of OrderDetails
                OrderDetails = JsonConvert.DeserializeObject<List<OrderDetails>>(orderDetailsJson);
            }
        }
    }
}
