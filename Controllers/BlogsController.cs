using Blog.Api.DtoModels;
using Blog.Api.Managers;
using Blog.Api.Providers;
using Blog.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BlogsController : ControllerBase
{
    private readonly BlogManager _manager;
    private readonly CommentManager _commentManager;
    private readonly PostManager _postManager;
    private readonly UserProvider _userProvider;

    public BlogsController(BlogManager manager, PostManager postManager, CommentManager commentManager, UserProvider userProvider)
    {
        _manager = manager;
        _postManager = postManager;
        _commentManager = commentManager;
        _userProvider = userProvider;
    }

    [HttpGet]
    public async Task<List<BlogModel>> GetAllTheBlogs()
    {
        return await _manager.GetAllTheBlogs();
    }

    [HttpPost]
    public async Task<BlogModel> CreateBlog(BlogDto model)
    {
        var blog = await _manager.CreateBlog(model);
        return ParseToBlogModel(blog);
    }
    private BlogModel ParseToBlogModel(Entities.Blog blog)
    {
            var model = new BlogModel()
            {
                Id = blog.Id,
                Posts = _postManager.ParseToListModel(blog.Posts),
                Description = blog.Description,
                CreatedDate = blog.CreateDateTime,
                Name = blog.Name,
                UserId = blog.UserId,
                UserName = _userProvider.UserName
            };
            return model;
    }
    
    [HttpGet("{Id}")]
    public async Task<BlogModel> GetBlogById(Guid Id)
    {
        var blog = await _manager.GetBlogById(Id);
        return blog;
    }
    [HttpDelete("{Id}")]
    public async Task DeleteBlog(Guid Id)
    {
        await _manager.DeleteBlog(Id);

    }

    [HttpPost("{blogId}/posts/")]
    public async Task<PostModel> CreatePost([FromForm]PostDto dto,Guid blogId)
    {
       
        var post = await _postManager.CreatePost(dto,blogId);
        return post;
    }

    [HttpGet("{blogId}/posts")]
    public async Task<List<PostModel>> GetPostsById(Guid blogId)
    {
        var list = await _postManager.GetAllPosts(blogId);
        return list;
    }
    [HttpGet("{blogId}/posts/{postId}")]
    public async Task<PostModel> GetPostById(Guid blogId,Guid postId)
    {
        var post = await _postManager.GetPostById (blogId,postId);
        return post;
    }

    [HttpDelete("{blogId}/posts/{postId}")]
    public async Task DeletePost(Guid blogId,Guid postId)
    {
        await _postManager.Delete(blogId, postId);
    }

    [HttpPost("{blogId}/posts/{postId}/comments")]
    public async Task<CommentModel> AddComment([FromForm] CommentDto dto,Guid blogId, Guid postId)
    {
        var comment = await _commentManager.AddComment(postId,blogId, dto);
        return comment;
    }
    [HttpGet("{blogId}/posts/{postId}/comments")]
    public async Task<List<CommentModel>> GetCommentById(Guid blogId, Guid postId)
    {
        var comment = await _commentManager.GetPostCommentsByPostId(blogId,postId);
        return comment;
    }
    
    /*[HttpGet("Update")]
    public async Task<Entities.Blog?> UpdateBlog(Entities.Blog blog)
    {
        var blogUpdated = await _manager.UpdateBlog(blog);
        return blogUpdated;
    }*/
}