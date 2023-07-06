using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Entities;
using Blog.Api.Exceptions;
using Blog.Api.Validators;
using Marketplace.Services.Products.FileServices;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Managers;

public class PostManager
{
    private readonly IdentityDbContext _context;
    private readonly BlogManager _blogManager;

    public PostManager(IdentityDbContext context, BlogManager blogManager)
    {
        _context = context;
        _blogManager = blogManager;
    }

    public async Task<PostModel> CreatePost(Guid Id, PostDto dto)
    {
        var blog =  await _blogManager.GetBlogById(Id);
        var postValidator = new PostValidators();
        var result = postValidator.Validate(dto);
        if (!result.IsValid)
        {
            throw new PostDtoIsNotValid("PostDto is not valid");
        }
        var post = new Post()
        {
            Title = dto.Title,
            Content = dto.Content,
            PhotoUrl = FileService.PostImages(dto.PostPhoto),
            BlogId = Id
        };
        blog.Posts.Add(post);
        await _context.SaveChangesAsync();
        return ParseToPostModel(post);
    }

    public async Task<List<PostModel>> GetAllPosts(Guid Id)
    {
        var blog = await _blogManager.GetBlogById(Id);
        var posts = blog.Posts;
        return ParseToListModel(posts);
    }
    public async Task<PostModel> GetPostById(Guid blogId,Guid postId)
    {
        var list = await GetAllPosts(blogId);
        var post = list.FirstOrDefault(i => i.Id == postId);
        if (post == null)
        {
            throw new PostNotFoundException(postId.ToString());
        }
        return post;
    }


    public async Task Delete(Guid blogId,Guid postId)
    {
        var blog = await GetPostById(blogId, postId);
        var post =  await _context.Posts.Where(i => i.Id == postId).FirstOrDefaultAsync();
        if (post == null)
        {
            throw new PostNotFoundException(postId.ToString());
        } 
        _context.Posts.Remove(post);
         await _context.SaveChangesAsync();
         
    }

  
    public PostModel ParseToPostModel(Post post)
    {
        var postModel = new PostModel()
        {
            Content = post.Content,
            Title = post.Title,
            BlogId = post.BlogId,
            CreatedDate = post.CreatedDate,
            Id = post.Id,
            PhotoUrl = post.PhotoUrl,
            Likes = post.Likes,
            Comments = post.Comments
        };
        return postModel;
    }

    public List<PostModel> ParseToListModel(List<Post> posts)
    {
        var list = new List<PostModel>();
        foreach (var post in posts)
        {
            list.Add(ParseToPostModel(post));
        }
        return list;
    }
}