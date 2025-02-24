using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RuthMo.Models;

namespace RuthMo.Data;

public class AppDbContext : IdentityDbContext<RuthMoUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}