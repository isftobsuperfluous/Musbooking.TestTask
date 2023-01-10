using Microsoft.EntityFrameworkCore;
using Musbooking.TestTask.Infrastructure.Entities;

namespace Musbooking.TestTask.Infrastructure;

public sealed class InMemoryDbContext : DbContext
{
    public InMemoryDbContext(DbContextOptions<InMemoryDbContext> options) : base(options)
    {
        
    }

    public DbSet<Equipment> Equipment { get; set; }
}