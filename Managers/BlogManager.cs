using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Exceptions;
using Blog.Api.Providers;
using Blog.Api.Validators;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Managers;

public class BlogManager
{
    private readonly IdentityDbContext _context;
    private readonly UserProvider _provider;
    

    public BlogManager( IdentityDbContext context, UserProvider provider)
    {
        _context = context;
        _provider = provider;
    }
    public async Task<List<BlogModel>> GetAllTheBlogs()
    {
        var list = await _context.Blogs.Include(i => i.Posts).ToListAsync();
        return ParseList(list);
    }

    public async Task<BlogModel> CreateBlog(BlogDto dto)
    {
        var blogValidator = new BlogValidator();
        var result = blogValidator.Validate(dto);
        if (!result.IsValid)
        {
            throw new BlogDtoIsNotValid("Blog dto is not valid");
        }
        var blog = new Entities.Blog()
        {
            Name = dto.BlogName,
            Description = dto.BlogDescription,
            UserId = _provider.UserId
        };
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return ParseToBlogModel(blog);
    }

    /*public async Task<BlogModel?> UpdateBlog()
    {

    }*/

    public async Task<BlogModel> GetBlogById(Guid Id)
    {
        var blog = IsExists(Id);
        return  ParseToBlogModel(blog);
    }

    public async Task DeleteBlog(Guid Id)
    {
        var blog = IsExists(Id);
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }


    public Entities.Blog IsExists(Guid Id)
    {
        var blog = _context.Blogs.FirstOrDefault(i => i.Id == Id);
        if (blog == null)
        {
            throw new BlogNotFoundException("Blog not Found");
        }
        return blog;
    }

  
    private List<BlogModel> ParseList(List<Entities.Blog> blogs)
    {
        var blogModels = new List<BlogModel>();
        foreach (var blog in blogs)
        {
            blogModels.Add(ParseToBlogModel(blog));
        }
        return blogModels;
    } 
    private BlogModel ParseToBlogModel(Entities.Blog blog)
    {
        var blogModel = new BlogModel()
        {
            Id = blog.Id,
            Name = blog.Name,
            Description = blog.Description,
            CreatedDate = blog.CreateDateTime,
            UserId = blog.UserId,
            UserName = _provider.UserName,
            Posts = blog.Posts,
        };
        return  blogModel;
    }
    
}

