namespace Blog.Api.Entities;

public class Blog
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreateDateTime {get;set;}= DateTime.UtcNow;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual List<Post> Posts { get; set; }

}