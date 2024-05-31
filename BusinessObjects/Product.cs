namespace BusinessObjects;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductCode { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string Brand{ get; set; }
    public string UrlImage { get; set; }
    public ProductStatus Status { get; set; }
    public double Discount { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
}
public enum ProductStatus
{
    Available,
    OutOfStock
}
