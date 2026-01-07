using ShopApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models;

public class ProductModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "image url belirtiniz")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "aciklama belirtiniz")]
    [StringLength(10000, MinimumLength = 20, ErrorMessage = "Ürün açıklaması minimum 20 karakter ve maksimum 100 karakter olmalıdır.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Fiyat belirtiniz")]
    [Range(typeof(decimal), "1", "10000", ErrorMessage = "fiyat {1} ve {2} arasında olmalıdır.")]
    public decimal? Price { get; set; }

    public List<Category> SelectedCategories { get; set; } = new();
}
