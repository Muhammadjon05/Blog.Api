namespace Blog.Api.DtoModels;

public class CommentModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime WrittenDate { get; set; }
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
}