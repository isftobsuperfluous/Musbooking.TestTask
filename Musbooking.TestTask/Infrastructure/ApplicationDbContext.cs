using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Infrastructure.Entities;

namespace Musbooking.TestTask.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Equipment> Equipment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipment>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<Equipment>()
            .Property(x => x.Name)
            .HasMaxLength(100);
        
        base.OnModelCreating(modelBuilder);
    }
}