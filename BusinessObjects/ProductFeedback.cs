namespace BusinessObjects;

public class ProductFeedback
{
    public int AccountId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Point { get; set; }
    public string Comment { get; set; }
    public DateTime CreateDate { get; set; } 
    public virtual Account? Account { get; set; }
    public virtual Product? Product { get; set; }
    public virtual Order? Order { get; set; }
}