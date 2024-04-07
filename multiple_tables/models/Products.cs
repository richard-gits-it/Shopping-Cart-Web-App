using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Products
    {
        public int ID { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] ImageData2 { get; set; }
        public string Name { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
        public string Description { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discontinued { get; set; }

    }
}
