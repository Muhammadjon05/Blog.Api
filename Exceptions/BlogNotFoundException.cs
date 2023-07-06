namespace Blog.Api.Exceptions;

public class BlogNotFoundException : Exception
{
    public BlogNotFoundException(string id): base($"Blog not Found with Id= {id}")
    {
        
    }
}