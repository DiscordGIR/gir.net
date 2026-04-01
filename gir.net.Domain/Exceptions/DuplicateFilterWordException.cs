namespace gir.net.Domain.Exceptions;

public class DuplicateFilerWordException : Exception
{
    public string FilterWordPhrase { get; }

    public DuplicateFilerWordException(string phrase, Exception? innerException = null)
        : base($"{phrase}' is already filtered.", innerException)
    {
        FilterWordPhrase = phrase;
    }
}
