namespace BusinessObjects;

public class Cart
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public CartStatuses Status { get; set; }
    public int Quantity { get; set; }
    public DateTime CreateDate { get; set; }
    public int AccountId { get; set; }  
    public int OrderDetailId { get; set; }
    public virtual OrderDetail? OrderDetail { get; set; }
    public virtual Product? Product { get; set; }
 }

public enum CartStatuses
{
    Ordered,
    StandBy,
    Wishlist
}