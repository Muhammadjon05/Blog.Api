using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Entities;
using Blog.Api.Exceptions;
using Blog.Api.Validators;
using Marketplace.Services.Products.FileServices;

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
        var blog =  _blogManager.IsExists(Id);
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

    public async Task<List<PostModel>> AllPosts(Guid Id)
    {
        var blog =  _blogManager.IsExists(Id);
        var posts = blog.Posts;
        return ParseToListModel(posts);
    }
    public async Task<PostModel> GetPostById(Guid blogId,Guid postId)
    {
        var post = await IsExist(blogId, postId);
        return post;
    }

    public async Task<PostModel> IsExist(Guid Id,Guid PostId)
    {
        var list = await AllPosts(Id);
        var post = list.FirstOrDefault(i => i.Id == PostId);
        if (post == null)
        {
            throw new PostNotFoundException($"With this{PostId} we could not find the post");
        }
        return post;
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