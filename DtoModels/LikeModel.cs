namespace Blog.Api.DtoModels;

public class LikeModel
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
}