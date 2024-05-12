using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;
public class Account
{
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public AccountRoles Role { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public virtual ICollection<Post>? Posts { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
    public virtual ICollection<MessageGroup>? MessageGroups { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    
}

public enum AccountRoles
{
    Admin, 
    Staff,
    Member
}