using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages.OrderHistory
{
    public class DetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int orderId { get; set; }

        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public DetailsModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public Orders Order { get; set; }
        public IList<OrderDetails> OrderDetails { get;set; }
        public double MerchandiseTotal { get; set; }
        public double ShippingCost { get; set; } = 5;
        //property for gst calculation 
        public double FederalTaxGST => (MerchandiseTotal + ShippingCost) * 0.05;
        public double TotalPrice => MerchandiseTotal + ShippingCost + FederalTaxGST;

        public async Task<IActionResult> OnGetAsync()
        {
            if (orderId == null)
            {
                return NotFound();
            }

            //get order details
            Order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == orderId);

            OrderDetails = await _context.OrderDetails
                .Include(o => o.Order)
                .Include(o => o.Product)
                .Where(o => o.OrderId == orderId)
                .ToListAsync();

            foreach (var item in OrderDetails)
            {
                MerchandiseTotal += (double)item.Product.UnitPrice * item.Quantity;
            }


            return Page();
        }
    }
}
