using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore;

public class EfCoreCategoryDal : EfCoreGenericRepository<Category, ShopContext>, ICategoryDal
{
    public void DeleteFromCategory(int categoryId, int productId)
    {
        using var context = new ShopContext();
        var cmd = $"DELETE FROM ProductCategory WHERE ProductId={productId} AND CategoryId={categoryId}";
        context.Database.ExecuteSqlRaw(cmd);
    }

    public Category? GetByIdWithProducts(int id)
    {
        using var context = new ShopContext();
        return context.Categories
                .Where(i => i.Id == id)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Product)
                .FirstOrDefault();
    }
}
