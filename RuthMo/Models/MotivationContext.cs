using Microsoft.EntityFrameworkCore;
using RuthMo.Extensions;

namespace RuthMo.Models;

public class MotivationContext : DbContext
{
    public MotivationContext(DbContextOptions<MotivationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasMany(m => m.Motivations)
            .WithOne(a => a.Author)
            .HasForeignKey(a => a.AuthorId);

        modelBuilder.Seed();
    }

    public DbSet<Motivation> Motivations { get; set; }
    public DbSet<Author> Authors { get; set; }
}