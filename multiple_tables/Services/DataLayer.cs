using multiple_tables.Data;
using multiple_tables.models;

namespace multiple_tables.Services
{
    public class DataLayer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;
            }

            var categories = new Categories[]
            {
                new Categories { Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
                new Categories { Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
                new Categories { Name = "Confections", Description = "Desserts, candies, and sweet breads" },
                new Categories { Name = "Dairy Products", Description = "Cheeses" },
                new Categories { Name = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
                new Categories { Name = "Meat/Poultry", Description = "Prepared meats" },
                new Categories { Name = "Produce", Description = "Dried fruit and bean curd" },
                new Categories { Name = "Seafood", Description = "Seaweed and fish" }
            };

            foreach (Categories c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var products = new Products[]
            {
                new Products { Name = "Chai", UnitPrice = 18.00M, CategoryId = 1, Description = "10 boxes x 20 bags", UnitsInStock = 39, Discontinued = false },
                new Products { Name = "Chang", UnitPrice = 19.00M, CategoryId = 1, Description = "24 - 12 oz bottles", UnitsInStock = 17, Discontinued = false },
                new Products { Name = "Aniseed Syrup", UnitPrice = 10.00M, CategoryId = 2, Description = "12 - 550 ml bottles", UnitsInStock = 13, Discontinued = false },
                new Products { Name = "Chef Anton's Cajun Seasoning", UnitPrice = 22.00M, CategoryId = 2, Description = "48 - 6 oz jars", UnitsInStock = 53, Discontinued = false },
                new Products { Name = "Chef Anton's Gumbo Mix", UnitPrice = 21.35M, CategoryId = 2, Description = "36 boxes", UnitsInStock = 0, Discontinued = true },
                new Products { Name = "Grandma's Boysenberry Spread", UnitPrice = 25.00M, CategoryId = 2, Description = "12 - 8 oz jars", UnitsInStock = 120, Discontinued = false },
                new Products { Name = "Uncle Bob's Organic Dried Pears", UnitPrice = 30.00M, CategoryId = 7, Description = "12 - 1 lb pkgs.", UnitsInStock = 15, Discontinued = false },
                new Products { Name = "Northwoods Cranberry Sauce", UnitPrice = 40.00M, CategoryId = 2, Description = "12 - 12 oz jars", UnitsInStock = 6, Discontinued = false },
                new Products { Name = "Mishi Kobe Niku", UnitPrice = 97.00M, CategoryId = 6, Description = "18 - 500 g pkgs.", UnitsInStock = 29, Discontinued = true },
                new Products { Name = "Ikura", UnitPrice = 31.00M, CategoryId = 8, Description = "12 - 200 ml jars", UnitsInStock = 31, Discontinued = false },
            };

            foreach (Products p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}
