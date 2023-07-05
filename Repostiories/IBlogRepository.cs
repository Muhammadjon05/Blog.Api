namespace Blog.Api.Repostiories;

public interface IBlogRepository
{
    
    Task<List<Entities.Blog>> GetAllTheBlogs();
    Task<Entities.Blog> CreateBlog(Entities.Blog blog);
    Task<Entities.Blog> GetBlogById(Guid Id);
    Task DeleteBlog(Guid Id);
    Task<Entities.Blog?> UpdateBlog(Entities.Blog blog);
}