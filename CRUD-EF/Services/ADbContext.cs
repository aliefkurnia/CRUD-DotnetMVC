using CRUD_EF.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_EF.Services;

public partial class ADbContext : DbContext
{
    public ADbContext()
    {
    }

    public ADbContext(DbContextOptions<ADbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<COM_CUSTOMER> COM_CUSTOMER { get; set; }

    public virtual DbSet<SO_ITEM> SO_ITEM { get; set; }

    public virtual DbSet<SO_ORDER> SO_ORDER { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=RCJKNB36;Database=testing;User ID=sa;Password=sa;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SO_ITEM>(entity =>
        {
            entity.Property(e => e.ItemName).HasDefaultValueSql("('')");
            entity.Property(e => e.Quantity).HasDefaultValueSql("((-99))");
            entity.Property(e => e.SoOrderId).HasDefaultValueSql("((-99))");
        });

        modelBuilder.Entity<SO_ORDER>(entity =>
        {
            entity.Property(e => e.Address).HasDefaultValueSql("('')");
            entity.Property(e => e.ComCustomerId).HasDefaultValueSql("('-99')");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("('1900-01-01')");
            entity.Property(e => e.OrderNo).HasDefaultValueSql("('')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
