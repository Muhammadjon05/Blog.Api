using Blog.Api.Entities;

namespace Blog.Api.DtoModels;

public class PostModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string PhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid BlogId { get; set; }
    public  List<Likes> Likes { get; set; }
    public List<Comment> Comments { get; set; }
}