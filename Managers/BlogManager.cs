using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Entities;
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
    public async Task<Entities.Blog> CreateBlog(BlogDto dto)
    {
        var blogValidator = new BlogValidator();
        var result = blogValidator.Validate(dto);
        if (!result.IsValid)
        {
            throw new BlogDtoIsNotValid("BlogDto is not valid");
        }
        var blog = new Entities.Blog()
        {
            Name = dto.BlogName,
            Description = dto.BlogDescription,
            UserId = _provider.UserId
        };
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
    public async Task<BlogModel> GetBlogById(Guid Id)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(i => i.Id == Id);
        if (blog == null)
        {
            throw new BlogNotFoundException(Id.ToString());
        }
        return  ParseToBlogModel(blog);
    }

    public async Task DeleteBlog(Guid Id)
    {
        var blog = await _context.Blogs.Where(i => i.Id == Id).FirstOrDefaultAsync();
        if (blog == null)
        {
            throw new BlogNotFoundException(Id.ToString());
        }
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
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
    public List<PostModel> ParseToPostModels(List<Post>? posts)
    {
        if (posts == null || posts.Count == 0)
        {
            return new List<PostModel>();
        }
        else
        {
            var models = new List<PostModel>();
            foreach (var post in posts)
            {
             models.Add(ParsePostToModel(post));
            }
            return models;
        }
    }
    public PostModel ParsePostToModel(Post? post)
    {
        var postModel = new PostModel()
        {
            Id = post.Id,
            CreatedDate = post.CreatedDate,
            Content = post.Content,
            BlogId = post.BlogId,
            Comments = ParseList(post.Comments),
            PhotoUrl = post.PhotoUrl,
            Title = post.Title,
            Likes = post.Likes,
        };
        return postModel;
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
            Posts = ParseToPostModels(blog.Posts),
        };
        return  blogModel;
    }
    public  CommentModel ParseCommentModel(Comment comment)
    {
        var model = new CommentModel()
        {
            Id = comment.Id,
            Text = comment.Text,
            WrittenDate = comment.WrittenDate,
            PostId = comment.PostId,
            UserId = comment.UserId
        };
        return model;
    } 
    public  List<CommentModel>? ParseList(List<Comment>? comments)
    {
        var model = new List<CommentModel>();
        foreach (var comment in comments)
        {
            model.Add(ParseCommentModel(comment));
        }
        return model;
    }
}

