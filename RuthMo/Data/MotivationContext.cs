using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Data;

public class MotivationContext : DbContext
{
    public DbSet<Motivation> Motivations { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(@"Host=localhost;Username=postgres;Password=postgres;Database=ruthmo");
    }
}