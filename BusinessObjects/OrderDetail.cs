namespace BusinessObjects;

public class OrderDetail
{
    public int OrderDetailId { get; set; }
    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }   
    public virtual ICollection<Cart>? Carts { get; set; }
}