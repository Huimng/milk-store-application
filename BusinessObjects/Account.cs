using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;
public class Account
{
    public int AccountId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public AccountRoles Role { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public virtual ICollection<Post>? Posts { get; set; }
    public virtual ICollection<Message>? ManagerMessages { get; set; } // Groups managed by this account
    public virtual ICollection<Message>? CustomerMessages { get; set; } // Groups where this account is a customer
    public virtual ICollection<MessageGroup>? ManagerGroups { get; set; } // Groups managed by this account
    public virtual ICollection<MessageGroup>? CustomerGroups { get; set; } // Groups where this account is a customer
    public virtual ICollection<Order>? Orders { get; set; }
    public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    
}

public enum AccountRoles
{
    Admin, 
    Staff,
    Member
}