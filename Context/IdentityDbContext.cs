using Blog.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Context;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Blogs)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);
        modelBuilder.Entity<Entities.Blog>()
            .HasMany(b => b.Posts)
            .WithOne(p => p.Blog)
            .HasForeignKey(p => p.BlogId);
        modelBuilder.Entity<User>().
            HasMany(u => u.Likes)
            .WithOne(l => l.User)
            .HasForeignKey(l => l.UserId);
        modelBuilder.Entity<User>()
            .HasMany(u => u.SavedPosts)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);
        modelBuilder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId);
        modelBuilder.Entity<Post>()
            .HasMany(p => p.SavedPosts)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Entities.Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Likes> Likes { get; set; }
    public DbSet<SavePost> SavedPosts { get; set; }

}