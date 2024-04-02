using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class CartProductsSession
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; } = default!;
        public int Quantity { get; set; }


    }
}
