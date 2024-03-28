using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly multiple_tables.Data.ApplicationDbContext _context;

        public IndexModel(multiple_tables.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Products> Products { get; set; } = default!;

        public async Task OnGetAsync()
        {

            //// Creating some categories
            //var categories = new Categories[]
            //{
            //new Categories { Name = "Electronics", Description = "Electronic devices" },
            //new Categories { Name = "Clothing", Description = "Apparel and garments" },
            //new Categories { Name = "Books", Description = "Literary works" }
            //// Add more categories as needed
            //};

            //// Creating some products
            //var products = new Products[]
            //{
            //new Products { Name = "Laptop", UnitPrice = 999.99m, CategoryId = 1, Description = "High performance laptop", UnitsInStock = 10, Discontinued = false },
            //new Products { Name = "T-shirt", UnitPrice = 19.99m, CategoryId = 2, Description = "Cotton t-shirt", UnitsInStock = 100, Discontinued = false },
            //new Products { Name = "Harry Potter and the Philosopher's Stone", UnitPrice = 15.99m, CategoryId = 3, Description = "Fantasy novel by J.K. Rowling", UnitsInStock = 50, Discontinued = false },
            //// New products
            //new Products { Name = "Smartphone", UnitPrice = 799.99m, CategoryId = 1, Description = "Latest smartphone model", UnitsInStock = 20, Discontinued = false },
            //new Products { Name = "Jeans", UnitPrice = 39.99m, CategoryId = 2, Description = "Slim-fit jeans", UnitsInStock = 80, Discontinued = false },
            //new Products { Name = "The Great Gatsby", UnitPrice = 12.99m, CategoryId = 3, Description = "Classic novel by F. Scott Fitzgerald", UnitsInStock = 30, Discontinued = false },
            //new Products { Name = "Bluetooth Speaker", UnitPrice = 49.99m, CategoryId = 1, Description = "Portable speaker with Bluetooth connectivity", UnitsInStock = 15, Discontinued = false },
            //// Two more products
            //new Products { Name = "Running Shoes", UnitPrice = 79.99m, CategoryId = 2, Description = "Lightweight running shoes", UnitsInStock = 40, Discontinued = false },
            //new Products { Name = "Digital Camera", UnitPrice = 299.99m, CategoryId = 1, Description = "High-resolution digital camera", UnitsInStock = 25, Discontinued = false }
            //// Add more products as needed
            //};

            //_context.Categories.AddRange(categories);
            //_context.Products.AddRange(products);
            //_context.SaveChanges();


            if (_context.Products != null)
            {
                Products = await _context.Products
                .Include(p => p.Category).ToListAsync();
            }
        }
    }
}