using Blog.Api.Context;
using Blog.Api.Providers;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Repostiories;

public class BlogRepository : IBlogRepository
{
    private readonly IdentityDbContext _context;
    private readonly UserProvider _provider;

    public BlogRepository(IdentityDbContext context, UserProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public async Task<List<Entities.Blog>> GetAllTheBlogs()
    {
        var blogs = await _context.Blogs.Include(i=>i.Posts).ToListAsync();
        return blogs;
    }

    public async Task<Entities.Blog> CreateBlog(Entities.Blog blog)
    {
        var blogEntity = new Entities.Blog()
        {
            Name = blog.Name,
            Description = blog.Description,
            CreateDateTime = DateTime.Now,
            UserId =_provider.UserId,
        };
        await _context.Blogs.AddAsync(blogEntity);
        await _context.SaveChangesAsync();
        return blogEntity;
    }

    public async Task<Entities.Blog> GetBlogById(Guid Id)
    {
        var blog = await _context.Blogs.Where(i=>i.Id == Id).FirstAsync();
        return blog;
    }

    public async Task DeleteBlog(Guid Id)
    {
        var blog = await GetBlogById(Id);
        if (blog == null)
        {
            return;
        };
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }

    public async Task<Entities.Blog?> UpdateBlog(Entities.Blog blog)
    {
        var blogById = await GetBlogById(blog.Id);
        if (blogById == null)
        {
            return null;
        }
        blogById.Description = blog.Description;
        blogById.Name = blog.Name;
        await _context.SaveChangesAsync();
        return blogById;
    }
}