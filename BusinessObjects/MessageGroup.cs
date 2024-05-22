namespace BusinessObjects;

public class MessageGroup
{
    public int MessageGroupId { get; set; }
    public string GroupName { get; set; }
    public int CustomerId { get; set; }
    public int ManagerId { get; set; }
    public MessageStatuses Status { get; set; }
    public virtual Account? Customer { get; set; }
    public virtual Account? Manager { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
}
public enum MessageStatuses
{
    Pending, 
    Opened,
    Closed
}