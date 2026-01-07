namespace ShopApp.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string UserId { get; set; } = string.Empty;

    public EnumOrderState OrderState { get; set; }
    public EnumPaymentTypes PaymentTypes { get; set; }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? OrderNote { get; set; }

    public string? PaymentId { get; set; }
    public string? PaymentToken { get; set; }
    public string? ConversationId { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}

public enum EnumOrderState
{
    waiting = 0,
    Unpaid = 1,
    Completed = 2
}

public enum EnumPaymentTypes
{
    CreditCart = 0,
    Eft = 1
}
