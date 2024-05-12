namespace BusinessObjects;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Contents { get; set; }
    public DateTime CreateDate { get; set; }
    public int CreateBy { get; set; }
    public PostStatuses Status { get; set; }
    public virtual Account? Account { get; set; }
    public virtual ICollection<Comment>? Comments { get; set; }
}
public enum PostStatuses
{
    Draft,
    Pending,
    Approved,
    Denied
}