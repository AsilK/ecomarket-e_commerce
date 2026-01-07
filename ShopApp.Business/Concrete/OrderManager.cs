using ShopApp.Business.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.Business.Concrete;

public class OrderManager : IOrderService
{
    private readonly IOrderDal _orderDal;

    public OrderManager(IOrderDal orderDal)
    {
        _orderDal = orderDal;
    }

    public void Create(Order entity)
    {
        _orderDal.Create(entity);
    }

    public List<Order> GetOrders(string? userId)
    {
        return _orderDal.GetOrders(userId);
    }
}
