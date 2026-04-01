namespace gir.net.Domain.Exceptions;

public class DuplicateTagNameException : Exception
{
    public string TagName { get; }

    public DuplicateTagNameException(string tagName, Exception? innerException = null)
        : base($"A tag named '{tagName}' already exists.", innerException)
    {
        TagName = tagName;
    }
}
