namespace Blog.Api.DtoModels;

public class PostDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile PostPhoto { get; set; }
}