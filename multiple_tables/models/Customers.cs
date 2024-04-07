using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace multiple_tables.models
{
    public class Customers : IdentityUser
    {
        //cart
        public Cart shoppingCart { get; set; }
        //initialize cart
        public Customers()
        {
            shoppingCart = new Cart();
        }



    }
}
