namespace Blog.Api.Exceptions;

public class PostNotFoundException : Exception
{
    public PostNotFoundException(string message): base($"With this {message} we could not find the post")
    {
        
    }

    
}