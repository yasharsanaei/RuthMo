using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RuthMo.Models;

public class MotivationContext : IdentityDbContext<User>
{
    public MotivationContext(DbContextOptions<MotivationContext> options) : base(options)
    {
    }
}