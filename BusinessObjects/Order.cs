namespace BusinessObjects;

public class Order
{
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public double TotalDiscount { get; set; }
    public double SubTotal { get; set; }
    public double GrandTotal { get; set; }
    public int CartId { get; set; }
    public string Address { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int AccountId { get; set; }
    public DeliveryTypes Type { get; set; }
    public PaymentType PaymentMethod { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    public virtual Account? Account { get; set; }
    public virtual OrderContact? OrderContact { get; set; }
}

public enum OrderStatus
{
    Pending,
    Canceled,
    Delivering,
    Failed,
    Succeeded
}

public enum DeliveryTypes
{
    Shipping,
    Directing
}

public enum PaymentType
{
    Cash,
    PayPal
}