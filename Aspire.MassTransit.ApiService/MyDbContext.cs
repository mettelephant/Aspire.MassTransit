using Microsoft.EntityFrameworkCore;

namespace Aspire.MassTransit.ApiService;

public class MyDbContext : DbContext
{
    public DbSet<MyEntity> MyEntities { get; set; } = null!;
    
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
        
    }
}