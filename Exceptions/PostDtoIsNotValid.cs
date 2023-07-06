namespace Blog.Api.Exceptions;

public class PostDtoIsNotValid : Exception
{
    public PostDtoIsNotValid(string message): base(message)
    {
        
    }


}