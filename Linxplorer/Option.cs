using Linxplorer.Enums;

namespace Linxplorer;

public class Option
{
    public Option(GeneralOption optionType, string? param)
    {
        OptionType = optionType;
        Param = param;
    }

    public Option(GeneralOption optionType)
    {
        OptionType = optionType;
    }

    public GeneralOption OptionType { get; }
    public string? Param { get; }
}