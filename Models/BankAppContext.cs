using Microsoft.EntityFrameworkCore;

namespace BankProject;

public class BankAppContext : DbContext
{

    public BankAppContext(DbContextOptions<BankAppContext> options) : base(options){ }

    public DbSet<ReceivableModel> Receivable { get; set; }
    public DbSet<AssignorModel> Assignor { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceivableModel>().HasKey(r => r.Id);
        modelBuilder.Entity<AssignorModel>().HasKey(a => a.AssignorId);
        base.OnModelCreating(modelBuilder);
    }
}
