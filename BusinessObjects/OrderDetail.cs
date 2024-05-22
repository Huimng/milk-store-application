namespace BusinessObjects;

public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int ProductId { get; set; }
    public virtual Order? Order { get; set; }  
    public virtual Product? Product { get; set; }
}