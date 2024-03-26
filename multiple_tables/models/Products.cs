using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Products
    {
        public int ID { get; set; }
        public string Name { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int CategoryId { get; set; }
        public Categories Category { get; set; } = default!;
        public string Description { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discontinued { get; set; }

        [NotMapped]
        public string CategoryName { get; set; } = default!;

    }
}
