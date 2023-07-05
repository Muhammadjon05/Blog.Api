namespace Blog.Api.Entities;

public class User
{
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public required string UserName { get; set; }
	public string PasswordHash { get; set; } = null!;
	public virtual List<Blog> Blogs { get; set; }
	public virtual List<Likes> Likes { get; set; }
	public virtual List<SavePost> SavedPosts { get; set; }
}