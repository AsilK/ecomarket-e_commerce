using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.DataAccess.Concrete.EfCore;

public class EfCoreOrderDal : EfCoreGenericRepository<Order, ShopContext>, IOrderDal
{
    public List<Order> GetOrders(string? userId)
    {
        using var context = new ShopContext();
        var orders = context.Orders
                        .Include(i => i.OrderItems)
                        .ThenInclude(i => i.Product)
                        .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            orders = orders.Where(i => i.UserId == userId);
        }

        return orders.ToList();
    }
}
