using JS17.API.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace JS17.API.Persistence;

public class WebDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<Post> Posts { get; set; }

    public WebDbContext(DbContextOptions<WebDbContext> options) : base(options) { }
}
