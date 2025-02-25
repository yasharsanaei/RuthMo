using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<RuthMoUser>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}