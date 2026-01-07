namespace ShopApp.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public List<ProductCategory> ProductCategories { get; set; } = new();
}
