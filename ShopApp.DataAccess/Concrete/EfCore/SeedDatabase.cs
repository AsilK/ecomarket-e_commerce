using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore;

public static class SeedDatabase
{
    public static void Seed()
    {
        using var context = new ShopContext();

        if (!context.Database.GetPendingMigrations().Any())
        {
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(Categories);
            }
            if (!context.Products.Any())
            {
                context.Products.AddRange(Products);
                context.AddRange(ProductCategories);
            }
            context.SaveChanges();
        }
    }

    private static readonly Category[] Categories =
    {
        new() { Name = "Sebze" },
        new() { Name = "Meyve" },
        new() { Name = "Bakliyat" }
    };

    private static readonly Product[] Products =
    {
        new() { Name = "Domates", Price = 5, ImageUrl = "1.jpg", Description = "<p>yüzde yüz yerli tire domat organik</p>" },
        new() { Name = "Havuç", Price = 3, ImageUrl = "2.jpg", Description = "<p>yüzde yüz yerli tire havuç organik</p>" },
        new() { Name = "Patates", Price = 4, ImageUrl = "3.jpg", Description = "<p>yüzde yüz yerli tire patates organik</p>" },
        new() { Name = "Salatalık", Price = 6, ImageUrl = "4.jpg", Description = "<p>yüzde yüz yerli tire salatalık organik</p>" },
        new() { Name = "Taze Fasulye", Price = 10, ImageUrl = "5.jpg", Description = "<p>yüzde yüz yerli tire fasulye organik</p>" },
        new() { Name = "meyve", Price = 3, ImageUrl = "1.jpg", Description = "<p>yüzde yüz yerli tire domat organik</p>" },
        new() { Name = "meyve", Price = 3, ImageUrl = "1.jpg", Description = "<p>yüzde yüz yerli tire domat organik</p>" }
    };

    private static readonly ProductCategory[] ProductCategories =
    {
        new() { Product = Products[0], Category = Categories[0] },
        new() { Product = Products[0], Category = Categories[2] },
        new() { Product = Products[1], Category = Categories[0] },
        new() { Product = Products[1], Category = Categories[1] },
        new() { Product = Products[2], Category = Categories[0] },
        new() { Product = Products[2], Category = Categories[2] },
        new() { Product = Products[3], Category = Categories[1] }
    };
}
