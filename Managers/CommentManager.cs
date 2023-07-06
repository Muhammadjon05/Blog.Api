using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Entities;
using Blog.Api.Migrations;
using Blog.Api.Providers;

namespace Blog.Api.Managers;

public class CommentManager
{
    private readonly IdentityDbContext _context;
    private readonly BlogManager _blogManager;
    private readonly PostManager _postManager;
    private readonly UserProvider _userProvider;
    public CommentManager(IdentityDbContext context, 
        BlogManager blogManager, 
        PostManager postManager, UserProvider userProvider)
    {
        _context = context;
        _blogManager = blogManager;
        _postManager = postManager;
        _userProvider = userProvider;
    }

    public async Task<CommentModel> AddComment(Guid postId,CommentDto dto)
    {
        var comment = new Comment()
        {
            Text = dto.Text,
            WrittenDate = DateTime.UtcNow,
            UserId = _userProvider.UserId,
            PostId =postId
        };
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return ParseToModel(comment);
    }

    public  CommentModel ParseToModel(Comment comment)
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
}