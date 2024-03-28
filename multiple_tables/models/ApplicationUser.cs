using Microsoft.AspNetCore.Identity;

namespace multiple_tables.models
{
    public class ApplicationUser : IdentityUser
    {
        public Cart shoppingCart { get; set; } = default!;

    }
}
