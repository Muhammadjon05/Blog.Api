using Blog.Api.DtoModels;
using Blog.Api.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BlogController : ControllerBase
{
    private readonly BlogManager _manager;

    public BlogController(BlogManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<List<BlogModel>> GetAllTheBlogs()
    {
        return await _manager.GetAllTheBlogs();
    }

    [HttpPost]
    public async Task<Entities.Blog> CreateBlog(BlogModel model)
    {
        var blog = _manager.CreateBlog(model);
        return await blog;
    }

    [HttpGet("GetById/{Id}")]
    public async Task<Entities.Blog> GetBlogById(Guid Id)
    {
        var blog = await _manager.GetBlogById(Id);
        return blog;
    }
    [HttpGet("Delete/{Id}")]
    public async Task DeleteBlog(Guid Id)
    {
        await _manager.DeleteBlog(Id);

    }
    [HttpGet("Update")]
    public async Task<Entities.Blog?> UpdateBlog(Entities.Blog blog)
    {
        var blogUpdated = await _manager.UpdateBlog(blog);
        return blogUpdated;
    }
}