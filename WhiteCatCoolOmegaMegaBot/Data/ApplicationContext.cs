using Microsoft.EntityFrameworkCore;
using WhiteCatCoolOmegaMegaBot.Models;

namespace WhiteCatCoolOmegaMegaBot.Data;

public class ApplicationContext : DbContext
{
    public DbSet<BServer> BServers { get; set; } = null!;
    public DbSet<BServerSetting> BServerSetting { get; set; } = null!;
    public DbSet<BUserActivity> BUserActivity { get; set; } = null!;
    
 
    public ApplicationContext()
    { 
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=whitecat;Username=postgres;Password=root");
    }
}