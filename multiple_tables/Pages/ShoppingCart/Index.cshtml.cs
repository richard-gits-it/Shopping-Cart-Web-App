﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace multiple_tables.Pages.ShoppingCart
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly multiple_tables.Data.ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(UserManager<IdentityUser> userManager, multiple_tables.Data.ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IList<CartProducts> CartProducts { get;set; } = default!;
        public double TotalPrice { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page if not authenticated
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get user id
            var userId = _userManager.GetUserId(User);

            // Check if user id is not null
            if (userId != null)
            {
                // Retrieve the user's cart including associated products
                var userCart = await _context.Cart
                    .Include(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // If user's cart doesn't exist, create a new one
                if (userCart == null)
                {
                    var newCart = new Cart { UserId = userId };
                    _context.Cart.Add(newCart);
                    await _context.SaveChangesAsync();

                    userCart = newCart;
                }

                // Check if cart products exist
                if (userCart.CartProducts != null)
                {
                    CartProducts = userCart.CartProducts.ToList();

                    // Calculate total price of the cart
                    TotalPrice = CartProducts.Sum(cp => cp.TotalPrice);
                }
                else
                {
                    // If cart products don't exist, initialize an empty list
                    CartProducts = new List<CartProducts>();
                    TotalPrice = 0; // Total price is zero if there are no products in the cart
                }
            }

            // Return the page
            return Page();
        }
        //OnPostRemoveFromCartAsync method
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int productId)
        {
            // Check if user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect to login page if not authenticated
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get user id
            var userId = _userManager.GetUserId(User);

            // Check if user id is not null
            if (userId != null)
            {
                // Retrieve the user's cart including associated products
                var userCart = await _context.Cart
                    .Include(c => c.CartProducts)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                // If user's cart doesn't exist, create a new one
                if (userCart == null)
                {
                    var newCart = new Cart { UserId = userId };
                    _context.Cart.Add(newCart);
                    await _context.SaveChangesAsync();

                    userCart = newCart;
                }

                // Check if cart products exist
                if (userCart.CartProducts != null)
                {
                    // Find the product in the cart
                    var product = userCart.CartProducts.FirstOrDefault(cp => cp.ProductId == productId);

                    // If the product exists, remove it from the cart
                    if (product != null)
                    {
                        _context.CartProducts.Remove(product);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            // Redirect to the shopping cart page
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCartItemAsync(int cartId, int productId)
        {
            var cartProduct = await _context.CartProducts.FindAsync(cartId, productId);

            if (cartProduct != null)
            {
                _context.CartProducts.Remove(cartProduct);
                await _context.SaveChangesAsync();
            }

            // Redirect back to the shopping cart page
            return RedirectToPage("/ShoppingCart/Index");
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(string cartId, string productId, int quantity, string operation)
        {
            if (operation == "increase")
            {
                quantity++;
            }
            else if (operation == "decrease")
            {
                quantity--;
            }
            // Convert cartId and productId to integers
            int cartIdInt = int.Parse(cartId);
            int productIdInt = int.Parse(productId);

            // Find the cart product
            var cartProduct = await _context.CartProducts.FindAsync(cartIdInt, productIdInt);

            if (cartProduct != null)
            {
                //check if quantity is less than 1
                if (quantity < 1)
                {
                    // Remove the product from the cart if quantity is less than 1
                    _context.CartProducts.Remove(cartProduct);
                }
                else
                {
                    // Update the quantity of the product in the cart
                    cartProduct.Quantity = quantity;
                }
                await _context.SaveChangesAsync();
            }

            // Redirect back to the shopping cart page
            return RedirectToPage("/ShoppingCart/Index");
        }
    }
}
