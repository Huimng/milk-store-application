namespace BusinessObjects;

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public string Brand{ get; set; }
    public string UrlImage { get; set; }
    public ProductStatus Status { get; set; }
    public virtual Cart? Cart { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
}
public enum ProductStatus
{
    Available,
    OutOfStock
}
