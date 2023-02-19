namespace Linxplorer;

public class Coincidence : IPrintable
{
    public const int Padding = 5;

    public static int CurrentWindowPos
    {
        get => _currentWindowPos;
        set => _currentWindowPos = value < 0 ? 0 : value;
    }

    public readonly List<Uri> Uris = new();
    public string Name { get; set; }
    public int Count { get; set; }
    public bool IsMarked;

    private static int _currentWindowPos;
    private readonly bool _isRoot;
    private bool _isPrinted;
    private bool _isSelected;
    private bool _countOff;
    private (int x, int y) _pos;


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
        Uris.Add(uri);
    }

    public override string ToString()
    {
        return Name + " Links count:" + Count;
    }

    public void Print(bool countOff = false)
    {
        _pos = Console.GetCursorPosition();
        _countOff = countOff;

        PrintNameCount();

        _isPrinted = true;
    }

    public void PrintNameCount()
    {
        var current = Console.GetCursorPosition();
        Console.SetCursorPosition(_pos.x, _pos.y);

        if (_isRoot)
        {
            Console.Write(Name);
        }
        else
        {
            if (IsMarked)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("X");
                Console.ResetColor();
            }

            if (_isSelected)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(">" + Name);

                if (CurrentWindowPos > _pos.y - Padding)
                {
                    CurrentWindowPos = _pos.y - Padding;
                }
                else if (CurrentWindowPos < _pos.y + Padding - Console.WindowHeight)
                {
                    CurrentWindowPos = _pos.y + Padding - Console.WindowHeight;
                }
            }
            else
            {
                Console.Write(Name);
            }

            if (Uris.Count > 0)
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("~");
            }

            if (Uris.Count == 0 || Count > 1)
            {
                if (!_countOff)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" Links count:" + Count);
                }
            }

            Console.ResetColor();
            Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft - 1));

            Console.SetCursorPosition(current.Left, current.Top);
            Console.WindowTop = CurrentWindowPos;
        }
    }

    public void SwitchColor()
    {
        if (_isPrinted)
        {
            _isSelected = !_isSelected;
            PrintNameCount();
        }
        else
        {
            throw new Exception("SwitchColor on no printed Coincidence");
        }
    }
}