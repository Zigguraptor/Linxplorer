namespace Linxplorer;

public class AnalysisResult
{
    public int TotalLinksCount { get; private set; }
    public ParsedFile[] ParsedFiles { get; private set; }

    public AnalysisResult(ParsedFile[] parsedFiles, int totalLinksCount)
    {
        ParsedFiles = parsedFiles;
        TotalLinksCount = totalLinksCount;
    }

    public void Sort()
    {
        if (ParsedFiles.Length <= 0) return;

        Array.Sort(ParsedFiles, (x, y) => string.Compare(x.FileName, y.FileName, StringComparison.Ordinal));

        foreach (var parsedFile in ParsedFiles)
            Array.Sort(parsedFile.Links,
                (x, y) => string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal));
    }

    public void Merge()
    {
        if (ParsedFiles.Length <= 0) return;

        var uris = ParsedFiles.First().Links.Distinct().ToArray();

        for (var i = 1; i < ParsedFiles.Length; i++)
            uris = ParsedFiles[i].Links.Union(uris).ToArray();

        TotalLinksCount = uris.Length;
        ParsedFiles = new[] { new ParsedFile("Unique result (merged)", uris) };
    }
}