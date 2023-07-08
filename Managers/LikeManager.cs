using Blog.Api.Context;
using Blog.Api.DtoModels;
using Blog.Api.Entities;
using Blog.Api.Exceptions;
using Blog.Api.Providers;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Managers;

public class LikeManager
{
    private readonly IdentityDbContext _context;
    private readonly UserProvider _provider;

    public LikeManager(IdentityDbContext context, UserProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public async Task<string> AddLikeByPostId(Guid blogId, Guid postId)
    {
        var userId = _provider.UserId;
        var blog = await _context.Blogs.Where(i => i.Id == blogId).FirstOrDefaultAsync();
        if (blog == null)
        {
            throw new BlogNotFoundException(blogId.ToString());
        }
        else
        {
            var post =  blog.Posts.Any(i => i.Id == postId);
            if (post)
            { 
                var isExist = await _context.Likes.Where(i => i.UserId == userId &&  i.PostId == postId).AnyAsync();
                var likeEntity = await _context.Likes.Where(i => i.UserId == userId && i.PostId == postId).FirstOrDefaultAsync();
                if (!isExist)
                {
                    var like = new Likes()
                    {
                        UserId = _provider.UserId,
                        PostId = postId
                    };
                    await _context.Likes.AddAsync(like);
                    await _context.SaveChangesAsync();
                    return "Liked";
                }
                else
                {
                    if (likeEntity != null)
                    {
                        _context.Likes.Remove(likeEntity);
                        await _context.SaveChangesAsync();
                    }
                    return "Like cancelled";
                }
            }
            else
            {
                throw new PostNotFoundException(postId.ToString());
            }
        }

    }

    public LikeModel ParseToLikeModel(Likes? like)
        {
            var model = new LikeModel()
            {
                Id = like.Id,
                PostId = like.PostId,
                UserId = like.PostId
            };

            return model;
        }
        public List<LikeModel> ParseToListLikeModel(List<Likes>? likes)
        {
            if (likes == null || likes.Count == 0)
            {
                return new List<LikeModel>();
            }
            else
            {
                var likeModel = new List<LikeModel>(); 
                foreach (var like in likes)
                {
                    likeModel.Add(ParseToLikeModel(like));
                }
                return likeModel;
            }
        }
 
    }
