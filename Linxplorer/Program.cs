using System.Diagnostics;
using System.Text.RegularExpressions;
using Linxplorer;
using Linxplorer.Enums;

var helpText = File.Exists("HelpText") ? File.ReadAllText("HelpText") : "HelpText file missing";

if (args.Length < 1)
{
    Process.Start("CMD.exe", "/k Linxplorer.exe --help");
    return;
}

var argsParser = new ArgsParser(args);

if (argsParser.ContainOption(GeneralOption.Help))
{
    Console.WriteLine(helpText);
    return;
}

if (argsParser.ContainOption(GeneralOption.Version))
{
    Console.WriteLine("\nVersion: 1.0-pre");
    return;
}

if (argsParser.FileNames.Length < 1)
    return;

var analysisResult = argsParser.ContainOption(GeneralOption.EndOfLink, out var opts) // if true, opts cannot be null.
    ? LinksParser.ExtractLinksByEnd(argsParser.FileNames, opts.Split(' '))
    : LinksParser.AnalyzeFiles(argsParser.FileNames, argsParser.ContainOption(GeneralOption.OnlyLinksOnFile));


if (analysisResult.ParsedFiles.Length < 1)
{
    Console.WriteLine("\n[ERROR] Files not found");
    return;
}

if (!argsParser.ContainOption(GeneralOption.WithoutHeaders))
{
    Console.WriteLine("\nAnalyzed " + analysisResult.ParsedFiles.Length + " file(s) and found " +
                      analysisResult.TotalLinksCount + " links.");
}

if (argsParser.ContainOption(GeneralOption.Merge))
    analysisResult.Merge();

if (argsParser.ContainOption(GeneralOption.Find, out var param))
{
    if (param is { Length: > 0 })
    {
        foreach (var item in analysisResult.ParsedFiles)
        {
            var filteredLinks = new List<Uri>();
            foreach (var link in item.Links)
            {
                if (Regex.IsMatch(link.ToString(), param))
                {
                    filteredLinks.Add(link);
                }
            }

            item.Links = filteredLinks.ToArray();
        }
    }
}

if (argsParser.ContainOption(GeneralOption.Sort))
    analysisResult.Sort();

if (argsParser.ContainOption(GeneralOption.OnlyLinks))
{
    foreach (var parsedFile in analysisResult.ParsedFiles)
    {
        if (!argsParser.ContainOption(GeneralOption.WithoutHeaders))
        {
            Console.WriteLine("\nIn file \u0022" + parsedFile.FileName + "\u0022 " + parsedFile.Links.Length +
                              " found links");
        }

        foreach (var link in parsedFile.Links)
        {
            Console.WriteLine(link);
        }
    }
}
else
{
    var tree = TreeBuilder.BuildFilesTree(analysisResult);

    if (argsParser.ContainOption(GeneralOption.SortByCount))
        tree.SortByCount();


    var explorer = new TreeExplorer(tree);
    explorer.Explore(argsParser.ContainOption(GeneralOption.PrintAll),
        argsParser.ContainOption(GeneralOption.WithoutHeaders),
        argsParser.ContainOption(GeneralOption.CountOff));
}