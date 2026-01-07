using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public CartController(
        IOrderService orderService, 
        ICartService cartService, 
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _orderService = orderService;
        _cartService = cartService;
        _userManager = userManager;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return RedirectToAction("Login", "Account");

        var cart = _cartService.GetCartByUserId(userId);
        if (cart == null) return View(new CartModel());

        return View(new CartModel()
        {
            CartId = cart.Id,
            CartItems = cart.CartItems.Select(i => new CartItemModel()
            {
                CartItemId = i.Id,
                ProductId = i.Product.Id,
                Name = i.Product.Name,
                Price = i.Product.Price ?? 0,
                ImageUrl = i.Product.ImageUrl,
                Quantity = i.Quantity
            }).ToList()
        });
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        var userId = _userManager.GetUserId(User);
        if (userId != null)
        {
            _cartService.AddToCart(userId, productId, quantity);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteFromCart(int productId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId != null)
        {
            _cartService.DeleteFromCart(userId, productId);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Checkout()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return RedirectToAction("Login", "Account");

        var cart = _cartService.GetCartByUserId(userId);
        if (cart == null) return RedirectToAction("Index");

        var orderModel = new OrderModel
        {
            CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            }
        };

        return View(orderModel);
    }

    [HttpPost]
    public IActionResult Checkout(OrderModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");

            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null) return RedirectToAction("Index");

            model.CartModel = new CartModel()
            {
                CartId = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            var payment = PaymentProcess(model);

            if (payment.Status == "success")
            {
                SaveOrder(model, payment, userId);
                ClearCart(cart.Id.ToString());
                return View("Success");
            }
        }

        return View(model);
    }

    private void SaveOrder(OrderModel model, Payment payment, string userId)
    {
        var order = new Order
        {
            OrderNumber = new Random().Next(111111, 999999).ToString(),
            OrderState = EnumOrderState.Completed,
            PaymentTypes = EnumPaymentTypes.CreditCart,
            PaymentId = payment.PaymentId,
            ConversationId = payment.ConversationId,
            OrderDate = DateTime.Now,
            FirstName = model.FirstName ?? string.Empty,
            LastName = model.LastName ?? string.Empty,
            Email = model.Email,
            Phone = model.Phone,
            Address = model.Address,
            UserId = userId
        };

        if (model.CartModel?.CartItems != null)
        {
            foreach (var item in model.CartModel.CartItems)
            {
                var orderitem = new ShopApp.Entities.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderitem);
            }
        }
        _orderService.Create(order);
    }

    private void ClearCart(string cartId)
    {
        _cartService.ClearCart(cartId);
    }

    private Payment PaymentProcess(OrderModel model)
    {
        var options = new Options
        {
            ApiKey = _configuration["Iyzico:ApiKey"] ?? "sandbox-ECA09KNTaes3IetV1JpkKpO1nL3pKrEo",
            SecretKey = _configuration["Iyzico:SecretKey"] ?? "sandbox-wDxAvfX3zjyNK0ZJvU7MFGl6zCcWSoB3",
            BaseUrl = "https://sandbox-api.iyzipay.com"
        };

        var request = new CreatePaymentRequest
        {
            Locale = Locale.TR.ToString(),
            ConversationId = Guid.NewGuid().ToString(),
            Price = model.CartModel?.TotalPrice().ToString("F0") ?? "0",
            PaidPrice = model.CartModel?.TotalPrice().ToString("F0") ?? "0",
            Currency = Currency.TRY.ToString(),
            Installment = 1,
            BasketId = model.CartModel?.CartId.ToString() ?? "0",
            PaymentChannel = PaymentChannel.WEB.ToString(),
            PaymentGroup = PaymentGroup.PRODUCT.ToString()
        };

        var paymentCard = new PaymentCard
        {
            CardHolderName = model.CardName,
            CardNumber = model.CardNumber,
            ExpireMonth = model.ExpirationMonth,
            ExpireYear = model.ExpirationYear,
            Cvc = model.Cvv,
            RegisterCard = 0
        };
        request.PaymentCard = paymentCard;

        var buyer = new Buyer
        {
            Id = "BY789",
            Name = "John",
            Surname = "Doe",
            GsmNumber = "+905350000000",
            Email = "email@email.com",
            IdentityNumber = "74300864791",
            LastLoginDate = "2015-10-05 12:43:35",
            RegistrationDate = "2013-04-21 15:12:09",
            RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
            Ip = "85.34.78.112",
            City = "Istanbul",
            Country = "Turkey",
            ZipCode = "34732"
        };
        request.Buyer = buyer;

        var shippingAddress = new Address
        {
            ContactName = "Jane Doe",
            City = "Istanbul",
            Country = "Turkey",
            Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
            ZipCode = "34742"
        };
        request.ShippingAddress = shippingAddress;

        var billingAddress = new Address
        {
            ContactName = "Jane Doe",
            City = "Istanbul",
            Country = "Turkey",
            Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
            ZipCode = "34742"
        };
        request.BillingAddress = billingAddress;

        var basketItems = new List<BasketItem>();

        if (model.CartModel?.CartItems != null)
        {
            foreach (var item in model.CartModel.CartItems)
            {
                var basketItem = new BasketItem
                {
                    Id = item.ProductId.ToString(),
                    Name = item.Name,
                    Category1 = "Sebze",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.Quantity * item.Price).ToString("F0")
                };
                basketItems.Add(basketItem);
            }
        }

        request.BasketItems = basketItems;

        return Payment.Create(request, options);
    }

    public IActionResult GetOrders()
    {
        var orders = _orderService.GetOrders(null);
        var orderListModel = new List<OrderListModel>();

        foreach (var order in orders)
        {
            var orderModel = new OrderListModel
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                OrderNote = order.OrderNote,
                Phone = order.Phone,
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                Address = order.Address,
                City = order.City,
                OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImageUrl
                }).ToList()
            };

            orderListModel.Add(orderModel);
        }

        return View(orderListModel);
    }
}