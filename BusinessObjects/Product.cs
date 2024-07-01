using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public class Product
{
    public int ProductId { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Required]
    public string ProductCode { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative value.")]
    public int Quantity { get; set; }
    [Required]
    public string Brand{ get; set; }
    public string UrlImage { get; set; }
    public ProductStatus Status { get; set; }
    [Required]
    public double Discount { get; set; }
    public DateTime CreatedDate { get; set; }
    public virtual ICollection<OrderDetail>? OrderDetail { get; set; }
    public virtual ICollection<Post>? Posts { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    public virtual ICollection<ProductLine>? ProductLines { get; set; }
}
public enum ProductStatus
{
    Available,
    OutOfStock,
    Deleted
}
