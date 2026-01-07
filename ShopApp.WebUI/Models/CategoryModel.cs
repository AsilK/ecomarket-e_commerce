using ShopApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShopApp.WebUI.Models;

public class CategoryModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "isim belirtiniz")]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "kategori ismi minimum 1 karakter ve maksimum 60 karakter olmalıdır.")]
    public string Name { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}
