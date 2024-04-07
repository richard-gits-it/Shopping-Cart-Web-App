using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Wishlists
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Product { get; set; }

        public string CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public Customers Customer { get; set; }
    }
}
