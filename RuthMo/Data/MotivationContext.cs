using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Data;

public class MotivationContext : DbContext
{
    public DbSet<Motivation> Motivations { get; set; } = null!;
    public DbSet<User> Authors { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost:5432;Username=postgres;Password=831373;Database=ruthmo");
    }
}