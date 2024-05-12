namespace BusinessObjects;

public class Message
{
    public int MessageId { get; set; }
    public int GroupId { get; set; }
    public string Content { get; set; }
    public int CustomerId { get; set; }
    public int ManagerId { get; set; }
    public virtual Account? Account { get; set; }
    public virtual MessageGroup? MessageGroup { get; set; }
}