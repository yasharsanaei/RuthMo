using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Data;

public class MotivationContext(DbContextOptions<MotivationContext> options) : IdentityDbContext(options)
{
    public DbSet<Motivation> Motivation { get; set; }
    public DbSet<User> User { get; set; }
}