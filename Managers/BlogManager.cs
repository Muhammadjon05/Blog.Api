using Blog.Api.DtoModels;
using Blog.Api.Repostiories;

namespace Blog.Api.Managers;

public class BlogManager
{
    protected readonly IBlogRepository _blogRepository;

    public BlogManager(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<List<BlogModel>> GetAllTheBlogs()
    {
       var blogs = await _blogRepository.GetAllTheBlogs();
       return ParseToModel(blogs);
    }

    public async Task<Entities.Blog> CreateBlog(BlogModel model)
    {
        if (model == null)
        {
            return null;
        }
        var blogModel = ConvertToBlog(model);
        var blog = await _blogRepository.CreateBlog(blogModel);
        return blog;
    }

    public async Task<Entities.Blog> GetBlogById(Guid Id)
    {
        var blog = await _blogRepository.GetBlogById(Id);
        return blog;
    }

    public async Task DeleteBlog(Guid Id)
    {
        await _blogRepository.DeleteBlog(Id);

    }

    public async Task<Entities.Blog?> UpdateBlog(Entities.Blog blog)
    {
        var blogUpdated = await _blogRepository.UpdateBlog(blog);
        return blogUpdated;
    }
    

    public Entities.Blog ConvertToBlog(BlogModel model)
    {
        var blog = new Entities.Blog()
        {
            Name = model.Name,
            Description = model.Description,
        };
        return blog;
    }

    public BlogModel ParseTo(Entities.Blog blog)
    {
        var model = new BlogModel()
        {
            Name = blog.Name,
            Description = blog.Description,
            UserId = blog.UserId
        };
        return model;
    }
    public List<BlogModel> ParseToModel(List<Entities.Blog> blogs)
    {
        var blogModels = new List<BlogModel>();
        foreach (var blog in blogs)
        {
            blogModels.Add(ParseTo(blog));
        }

        return blogModels;
    }
}

