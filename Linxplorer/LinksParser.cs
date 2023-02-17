using System.Text.RegularExpressions;

namespace Linxplorer;

public static class LinksParser
{
    private static readonly string[] LinkPatterns = new[]
    {
        @"(https?:\/\/)?([\w-]{1,32}\.[\w-]{1,32})[^\s@,'"">;]*",
        @"(https?:\/\/)?[\w-]+(\.[\w-]+)+(:\d{1,5})?(?(\/)\/([^\s@><""',\/]+\/?)*|)"
    };

    public static AnalysisResult AnalyzeFiles(string[] fieNames, bool extractLinksOnFiles)
    {
        if (fieNames == null) throw new ArgumentNullException(nameof(fieNames));

        var totalLinksCount = 0;
        var parsedFiles = new List<ParsedFile>();
        foreach (var fieName in fieNames)
        {
            if (!File.Exists(fieName)) continue;

            var links = GetLinksFromText(File.ReadAllText(fieName), extractLinksOnFiles);
            parsedFiles.Add(new ParsedFile(fieName, links));
            totalLinksCount += links.Length;
        }

        return new AnalysisResult(parsedFiles.ToArray(), totalLinksCount);
    }

    public static AnalysisResult ExtractLinksByEnd(string[] fieNames, string[] patterns)
    {
        if (fieNames == null) throw new ArgumentNullException(nameof(fieNames));

        var totalLinksCount = 0;
        var parsedFiles = new List<ParsedFile>();
        foreach (var fieName in fieNames)
        {
            if (!File.Exists(fieName)) continue;

            var links = GetLinksFromText(File.ReadAllText(fieName), patterns);
            parsedFiles.Add(new ParsedFile(fieName, links));
            totalLinksCount += links.Length;
        }

        return new AnalysisResult(parsedFiles.ToArray(), totalLinksCount);
    }

    private static Uri[] GetLinksFromText(string text, bool extractLinksOnFiles)
    {
        var matches = Regex.Matches(text, LinkPatterns[1]);
        var links = new List<Uri>();
        Uri? uri;
        if (extractLinksOnFiles)
        {
            foreach (Match match in matches)
            {
                if (!Uri.TryCreate(match.ToString(), UriKind.Absolute, out uri)) continue;
                if (Regex.IsMatch(uri.AbsolutePath, @"^.*\.[^/]+$"))
                    links.Add(uri);
            }
        }
        else
        {
            foreach (Match match in matches)
            {
                if (Uri.TryCreate(match.ToString(), UriKind.Absolute, out uri))
                    links.Add(uri);
            }
        }

        return links.ToArray();
    }

    private static Uri[] GetLinksFromText(string text, string[] patterns)
    {
        var matches = Regex.Matches(text, LinkPatterns[1]);
        var links = new List<Uri>();
        Uri? uri;

        foreach (Match match in matches)
        {
            if (!Uri.TryCreate(match.ToString(), UriKind.Absolute, out uri)) continue;
            if (patterns.Any(pattern => uri.AbsolutePath.EndsWith(pattern)))
                links.Add(uri);
        }

        return links.ToArray();
    }
}