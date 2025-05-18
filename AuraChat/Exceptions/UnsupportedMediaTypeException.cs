namespace AuraChat.Exceptions;

public class UnsupportedMediaTypeException : Exception
{
    public UnsupportedMediaTypeException(string message) : base(message) { }
    public UnsupportedMediaTypeException(string message, Exception inner) : base(message, inner) { }
}
