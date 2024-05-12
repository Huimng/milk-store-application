namespace BusinessObjects;

public class Order
{
    public int OrderId { get; set; }
    public int CartId { get; set; }
    public string Address { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentType Payment { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
}

public enum OrderStatus
{
    Pending,
    Canceled,
    Delivering,
    Failed,
    Succeeded
}

public enum PaymentType
{
    Cash,
    PayPal
}