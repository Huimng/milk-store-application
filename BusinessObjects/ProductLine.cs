namespace BusinessObjects;

public class ProductLine
{
    public int ProductLineId { get; set; }
    public int ProductId { get; set; }
    public DateTime ExpireDate { get; set; }
    public string? AgeGroup { get; set; } // "0-6 months", "6-12 months", "1-2 years"

    public virtual Product? Product { get; set; }
}