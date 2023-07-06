namespace Blog.Api.Exceptions;

public class BlogDtoIsNotValid : Exception
{
    public BlogDtoIsNotValid(string message): base(message)
    {
        
    }

}