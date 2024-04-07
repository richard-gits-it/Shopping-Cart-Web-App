using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public Customers User { get; set; } = default!;

        // Initialize CartProducts collection in the constructor
        public Cart()
        {
            CartProducts = new List<CartProducts>();
        }
        public ICollection<CartProducts> CartProducts { get; set; }

    }
}
