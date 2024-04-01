using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace multiple_tables.models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        public ICollection<CartProducts> CartProducts { get; }
    }
}
