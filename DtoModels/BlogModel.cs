using Blog.Api.Entities;

namespace Blog.Api.DtoModels;

public class BlogModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } 
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public  List<PostModel>? Posts { get; set; }
}