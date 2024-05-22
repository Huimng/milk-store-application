namespace BusinessObjects;

public class OrderContact
{
    public int OrdContacId { get; set; }
    public int OrderId { get; set; }
    public string CustomerName { get; set; }    
    public string Phone { get; set; }
    public string Province { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string HouseNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public virtual Order? Order { get; set; }
}