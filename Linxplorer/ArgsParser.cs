using Linxplorer.Enums;

namespace Linxplorer;

public class ArgsParser
{
    private string[] Args { get; }
    public readonly string[] FileNames;
    public readonly Option[] Options;

    private static readonly Dictionary<char, GeneralOption> GeneralOptionsKeys = new Dictionary<char, GeneralOption>
    {
        { 'a', GeneralOption.PrintAll },
        { 'h', GeneralOption.WithoutHeaders },
        { 'l', GeneralOption.OnlyLinks },
        { 'f', GeneralOption.OnlyLinksOnFile },
        { 'c', GeneralOption.CountOff },
        { 's', GeneralOption.Sort },
        { 'b', GeneralOption.SortByCount },
        { 'm', GeneralOption.Merge }
    };

    private static readonly Dictionary<char, GeneralOption> GeneralOptionsNoIgnoreCaseKeys =
        new Dictionary<char, GeneralOption>
        {
            { 'C', GeneralOption.SortByCount },
        };

    private static readonly Dictionary<string, GeneralOption> GeneralOptions = new Dictionary<string, GeneralOption>
    {
        { "--All", GeneralOption.PrintAll },
        { "--Headers-Off", GeneralOption.WithoutHeaders },
        { "--Links", GeneralOption.OnlyLinks },
        { "--Files", GeneralOption.OnlyLinksOnFile },
        { "--Count-Off", GeneralOption.CountOff },
        { "--Sort", GeneralOption.Sort },
        { "--Sort-Count", GeneralOption.SortByCount },
        { "--Marge", GeneralOption.Merge },

        { "--help", GeneralOption.Help },
        { "--version", GeneralOption.Version }
    };

    private static readonly Dictionary<string, GeneralOption> GeneralOptionsParams =
        new Dictionary<string, GeneralOption>
        {
            { "--files=", GeneralOption.OnlyLinksOnFile },
            { "--end-of-link=", GeneralOption.EndOfLink },
        };

    public ArgsParser(string[] args)
    {
        Args = args;

        var fileNames = new List<string>();
        var options = new List<Option>();

        var i = 0;
        for (; i < Args.Length; i++)
        {
            if (args[i].StartsWith('-'))
            {
                AddOption(args[i]);
            }
            else
            {
                break;
            }
        }

        for (; i < args.Length; i++)
        {
            fileNames.Add(args[i]);
        }

        FileNames = fileNames.ToArray();
        Options = options.ToArray();

        void AddOption(string arg)
        {
            if (arg[1] == '-')
            {
                foreach (var option in GeneralOptionsParams)
                {
                    if (!arg.StartsWith(option.Key)) continue;

                    if (arg.Length > option.Key.Length)
                    {
                        options.Add(new Option(option.Value, arg[option.Key.Length..]));
                        return;
                    }
                }

                foreach (var option in GeneralOptions)
                {
                    if (IsCommand(option.Key, option.Value))
                    {
                        fileNames.Add(arg);
                        return;
                    }
                }
            }
            else
            {
                foreach (var option in GeneralOptionsKeys)
                {
                    ArgContains(option.Key, option.Value);
                }

                foreach (var option in GeneralOptionsNoIgnoreCaseKeys)
                {
                    ArgContains(option.Key, option.Value, false);
                }
            }

            bool IsCommand(string command, GeneralOption option)
            {
                if (arg != command) return false;

                options.Add(new Option(option));
                return true;
            }

            void ArgContains(char commandKey, GeneralOption option, bool ignoreCase = true)
            {
                if (ignoreCase)
                {
                    if (arg.Contains(commandKey, StringComparison.OrdinalIgnoreCase))
                    {
                        options.Add(new Option(option));
                    }
                }
                else
                {
                    if (arg.Contains(commandKey, StringComparison.Ordinal))
                    {
                        options.Add(new Option(option));
                    }
                }
            }
        }
    }

    public bool ContainOption(GeneralOption option)
    {
        foreach (var opt in Options)
        {
            if (opt.OptionType == option)
                return true;
        }

        return false;
    }

    public bool ContainOption(GeneralOption option, out string? param)
    {
        foreach (var opt in Options)
        {
            if (opt.OptionType != option) continue;

            param = opt.Param;
            return true;
        }

        param = default;
        return false;
    }
}