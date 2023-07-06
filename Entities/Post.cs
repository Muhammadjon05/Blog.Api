namespace Blog.Api.Entities;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string PhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }= DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set;}
    public Guid BlogId { get; set; }
    public  virtual Blog Blog { get; set; }
    public virtual List<Likes> Likes { get; set; }
    public virtual List<SavePost> SavedPosts { get; set; }
}