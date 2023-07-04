using Blog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Context;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users => Set<User>();
}