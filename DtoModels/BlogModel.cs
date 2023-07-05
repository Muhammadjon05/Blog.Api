namespace Blog.Api.DtoModels;

public class BlogModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Guid UserId { get; set; }
}