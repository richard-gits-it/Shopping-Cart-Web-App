using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Orders Order { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
