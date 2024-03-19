namespace multiple_tables.models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; }
        public ICollection<Products> Products { get; }
    }
}
