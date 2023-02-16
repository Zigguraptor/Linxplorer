namespace Linxplorer;

public class ParsedFile
{
    public readonly string FileName;
    public readonly Uri[] Links;

    public ParsedFile(string fileName, Uri[] links)
    {
        FileName = fileName;
        Links = links;
    }
}
