namespace Linxplorer;

public class Coincidence : IPrintable
{
    public const int Padding = 5;

    public static int CurrentWindowPos
    {
        get => _currentWindowPos;
        set => _currentWindowPos = value < 0 ? 0 : value;
    }

    public string Name { get; set; }
    public int Count { get; set; }

    public readonly Uri Uri = null!;


    private readonly bool _isRoot;
    private bool _isPrinted;
    private bool _isSelected;
    private (int x, int y) _pos;
    private static int _currentWindowPos;

    public Coincidence(string name, bool isRoot)
    {
        Name = name;
        _isRoot = isRoot;
    }

    public Coincidence(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public Coincidence(string name, int count, Uri uri)
    {
        Name = name;
        Count = count;
        Uri = uri;
    }

    public override string ToString()
    {
        return Name + " Links count:" + Count;
    }

    public void Print(bool countOff = false)
    {
        _pos = Console.GetCursorPosition();

        PrintNameCount(countOff);

        _isPrinted = true;
    }

    private void PrintNameCount(bool countOff = false)
    {
        if (_isRoot)
        {
            Console.Write(Name);
        }
        else
        {
            Console.Write(Name);
            if (Uri != null)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("~");
            }

            if (Uri == null || Count > 1)
            {
                if (!countOff)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" Links count:" + Count);
                }
            }

            Console.ResetColor();
            Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft - 1));
        }
    }

    public void SwitchColor(bool countOff)
    {
        if (_isPrinted)
        {
            var current = Console.GetCursorPosition();
            if (_isSelected)
            {
                Console.SetCursorPosition(_pos.x, _pos.y);
                PrintNameCount(countOff);
                Console.SetCursorPosition(current.Left, current.Top);
                _isSelected = false;
            }
            else
            {
                Console.SetCursorPosition(_pos.x, _pos.y);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(">");
                PrintNameCount(countOff);
                Console.SetCursorPosition(current.Left, current.Top);

                if (CurrentWindowPos > _pos.y - Padding)
                {
                    CurrentWindowPos = _pos.y - Padding;
                }
                else if (CurrentWindowPos < _pos.y + Padding - Console.WindowHeight)
                {
                    CurrentWindowPos = _pos.y + Padding - Console.WindowHeight;
                }

                _isSelected = true;
            }

            Console.WindowTop = CurrentWindowPos;
        }
        else
        {
            throw new Exception("SwitchColor on no printed Coincidence");
        }
    }
}