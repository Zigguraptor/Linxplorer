using System.Diagnostics;
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
    Console.WriteLine("\nVersion: 0.0.1 alpha for friends.");
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