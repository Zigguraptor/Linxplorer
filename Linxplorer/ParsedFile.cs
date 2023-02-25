namespace Linxplorer;

public class ParsedFile
{
    public readonly string FileName;
    public Uri[] Links;

    public ParsedFile(string fileName, Uri[] links)
    {
        FileName = fileName;
        Links = links;
    }
}
