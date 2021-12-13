using System;

public class TooSlowException : Exception
{
    public TooSlowException()
    {
    }

    public TooSlowException(string message)
        : base(message)
    {
    }

    public TooSlowException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
