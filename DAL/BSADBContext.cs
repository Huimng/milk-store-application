using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DTO;

public partial class BSADBContext : DbContext
{
    //public BSADBContext(DbContextOptions<BSADBContext> options) : base(options){}
    //public BSADBContext() { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageGroup> MessageGroups { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<OrderContact> OrderContacts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductFeedback> ProductFeedbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure database provider and connection string
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=BabyStore;Username=postgres;Password=17011206;Integrated Security=true;");
    }
}

public partial class BSADBContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        #region Account

        // Configure entity mappings and relationships
        modelBuilder.Entity<Account>(entity =>
        {
            // Configure primary key
            entity.HasKey(e => e.AccountId);
            entity.Property(e => e.AccountId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Configure properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(32);

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(32);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(e => e.Role)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdateDate)
                .IsRequired();
        });

        #endregion

        #region Post

        // Configure Post entity
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId);
            entity.Property(e => e.PostId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Column lengths and configurations
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Contents)
                .IsRequired();

            entity.Property(e => e.CreateDate)
                .IsRequired();

            entity.Property(e => e.CreateBy)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            // Configure relationships
            entity.HasOne(e => e.Account)
                .WithMany(a => a.Posts)
                .HasForeignKey(e => e.CreateBy)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Product)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        #endregion

        #region MessageGroup

        //Configure MessageGroup entity
        modelBuilder.Entity<MessageGroup>(entity =>
        {
            //Configure Primary Key
            entity.HasKey(e => e.MessageGroupId);
            entity.Property(e => e.MessageGroupId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            // Column lengths and configurations
            entity.Property(e => e.GroupName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.CustomerId)
                .IsRequired();

            entity.Property(e => e.ManagerId)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.HasOne(e => e.Customer)
                .WithMany(a => a.CustomerGroups)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(a => a.AccountId);

            entity.HasOne(e => e.Manager)
                .WithMany(a => a.ManagerGroups)
                .HasForeignKey(e => e.ManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(a => a.AccountId);
        });

        #endregion

        #region Message

        // Configure MessageGroup entity
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId);
            entity.Property(e => e.MessageId)
                .ValueGeneratedOnAdd()
                .IsRequired();


            // Column lengths and configurations
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.GroupId)
                .IsRequired();

            entity.Property(e => e.CustomerId)
                .IsRequired();

            entity.Property(e => e.ManagerId)
                .IsRequired();

            entity.HasOne(e => e.MessageGroup)
                .WithMany(g => g.Messages)
                .HasForeignKey(e => e.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                .WithMany(a => a.CustomerMessages)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(k => k.AccountId);

            entity.HasOne(e => e.Manager)
                .WithMany(a => a.ManagerMessages)
                .HasForeignKey(e => e.ManagerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict)
                .HasPrincipalKey(k => k.AccountId);
        });

        #endregion

        #region Comment

        // Configure Comment entity
        modelBuilder.Entity<Comment>(entity =>
        {
            //PK
            entity.HasKey(e => e.CommentId);
            entity.Property(e => e.CommentId)
                .ValueGeneratedOnAdd()
                .IsRequired();


            // Column lengths and configurations
            entity.Property(e => e.Content)
                .IsRequired();

            entity.Property(e => e.CreateDate)
                .IsRequired();

            // Configure relationship with Post
            entity.HasOne(e => e.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(e => e.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // or Restrict/Cascade as needed
        });

        #endregion

        #region ProductFeedback

        // Configure ProductFeedback entity
        modelBuilder.Entity<ProductFeedback>(entity =>
        {
            entity.HasKey(k => new { k.AccountId, k.ProductId, k.OrderId });

            // Column lengths and configurations
            entity.Property(e => e.Comment)
                .IsRequired();

            // Configure relationships
            entity.HasOne(e => e.Account)
                .WithMany(a => a.ProductFeedbacks)
                .HasForeignKey(e => e.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Order)
                .WithMany(o => o.ProductFeedbacks)
                .HasForeignKey(e => e.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Product)
                .WithMany(a => a.ProductFeedbacks)
                .HasForeignKey(e => e.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region OrderDetail

        // Configure OrderDetail entity
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId);
            entity.Property(e => e.OrderDetailId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.OrderId)
                .IsRequired();

            entity.Property(e => e.TotalPrice)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();

            entity.Property(e => e.ProductId)
                .IsRequired();

            // Configure relationship with Order
            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(c => c.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship with Product
            entity.HasOne(e => e.Product)
                .WithOne(o => o.OrderDetail)
                .HasForeignKey<Product>(c => c.ProductId)
                .IsRequired();
        });

        #endregion

        #region OrderContact

        modelBuilder.Entity<OrderContact>(entity =>
        {
            entity.HasKey(e => e.OrdContacId);
            entity.Property(e => e.OrdContacId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.CustomerName)
                .IsRequired();

            entity.Property(e => e.Phone)
                .IsRequired();

            entity.Property(e => e.Province)
                .IsRequired();

            entity.Property(e => e.City)
                .IsRequired();

            entity.Property(e => e.District)
                .IsRequired();

            entity.Property(e => e.HouseNumber)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();

            // Define relationship with Order
            entity.HasOne(oc => oc.Order)
                .WithOne(o => o.OrderContact)
                .HasForeignKey<OrderContact>(oc => oc.OrderId)
                .IsRequired();
        });

        #endregion

        #region Order

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.TotalDiscount)
                .IsRequired();

            entity.Property(e => e.SubTotal)
                .IsRequired();

            entity.Property(e => e.GrandTotal)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .IsRequired();

            entity.Property(e => e.UpdatedDate)
                .IsRequired();

            entity.Property(e => e.AccountId)
                .IsRequired();

            entity.Property(e => e.Type)
                .IsRequired();

            //Define relationship
            entity.HasOne(e => e.Account)
                .WithMany(o => o.Orders)
                .HasForeignKey(c => c.AccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });

        #endregion

        #region Product

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            // Column lengths and configurations
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.ProductCode)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.Brand)
                .HasMaxLength(255);

            entity.Property(e => e.UrlImage)
                .HasMaxLength(255);

            entity.Property(e => e.Discount)
                .IsRequired();

            entity.Property(e => e.CreatedDate)
                .IsRequired();
        });

        #endregion
    }
}