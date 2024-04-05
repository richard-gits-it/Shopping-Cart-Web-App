using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class CartProducts
    {
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; } = default!;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; } = default!;
        public int Quantity { get; set; }

        //get total price of product
        public double TotalPrice
        {
            get
            {
                return (double)Product.UnitPrice * Quantity;
            }
        }
    }
}
