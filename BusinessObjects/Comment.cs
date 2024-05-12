namespace BusinessObjects;

public class Comment
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreateDate { get; set; }
    public virtual Post? Post { get; set; }
}