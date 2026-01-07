using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore;

public class EfCoreCartDal : EfCoreGenericRepository<Cart, ShopContext>, ICartDal
{
    public override void Update(Cart entity)
    {
        using var context = new ShopContext();
        context.Carts.Update(entity);
        context.SaveChanges();
    }

    public Cart? GetByUserId(string userId)
    {
        using var context = new ShopContext();
        return context
                    .Carts
                    .Include(i => i.CartItems)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefault(i => i.UserId == userId);
    }

    public void DeleteFromCart(int cartId, int productId)
    {
        using var context = new ShopContext();
        var cmd = $"DELETE FROM CartItem WHERE CartId={cartId} AND ProductId={productId}";
        context.Database.ExecuteSqlRaw(cmd);
    }

    public void ClearCart(string cartId)
    {
        using var context = new ShopContext();
        var cmd = $"DELETE FROM CartItem WHERE CartId={cartId}";
        context.Database.ExecuteSqlRaw(cmd);
    }
}
