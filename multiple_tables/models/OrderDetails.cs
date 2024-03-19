namespace multiple_tables.models
{
    public class OrderDetails
    {
        public int OrderId { get; set; } = default!;
        public Orders Order { get; set; } = default!;
        public int ProductId { get; set; } = default!;
        public Products Product { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
    }
}
