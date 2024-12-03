namespace Data.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string s) : base(s)
    {
        
    }
}