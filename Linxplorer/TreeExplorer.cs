namespace Linxplorer;

public class TreeExplorer
{
    public TreeExplorer(NodeTree root)
    {
        Root = root;
        BackStack = new Stack<NodeTree>();
        BackStack.Push(Root);
    }

    private bool _exploreAll;
    private bool _headersOff;


    private NodeTree Root { get; }
    private Stack<NodeTree> BackStack { get; }
    private readonly List<NodeTree> _markedNods = new();

    public void Explore(bool exploreAll = false, bool headersOff = false, bool countOff = false)
    {
        if (!headersOff)
        {
            Console.Write('\n');
            PrintHintButton("[Esc] - Exit");
            Console.Write("   ");
            PrintHintButton("[Enter] - Print branch");
            Console.Write("   ");
            PrintHintButton("[x] - mark element");
            // Console.Write("   ");
            // PrintHintButton("[a] - mark all"); //TODO
            Console.Write("\n\n");
            PrintHintButton("[↑] - Up");
            Console.Write("   ");
            PrintHintButton("[↓] - Down");
            Console.Write("   ");
            PrintHintButton("[→] - Next element");
            Console.Write("   ");
            PrintHintButton("[←] - Previous element");
            Console.Write("\n\n");
        }

        _exploreAll = exploreAll;
        _headersOff = headersOff;
        SelectorEnable(countOff);
    }

    private static void PrintHintButton(string hint)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(hint);
        Console.ResetColor();
    }

    private void SelectorEnable(bool countOff = false)
    {
        var treeStartPos = Console.CursorTop;
        Coincidence.CurrentWindowPos = treeStartPos - 8;

        if (_exploreAll)
        {
            BackStack.Peek().PrintNodeTree(countOff);
        }
        else
        {
            BackStack.Peek().PrintOneLayer(countOff);
        }

        if (BackStack.Peek().Children.Count > 0)
        {
            BackStack.Push(BackStack.Peek().Children.First());
            BackStack.Peek().Content.SwitchColor();
        }
        else
        {
            Console.WriteLine("No links :(");
            return;
        }

        var selectedElementNumber = 1;

        // Console.CursorVisible = false; // TODO
        var read = true;

        while (read)
        {
            var k = Console.ReadKey(true);
            if (k.Key == ConsoleKey.Escape) break;

            switch (k.Key)
            {
                case ConsoleKey.DownArrow:
                    SelectNext();
                    break;
                case ConsoleKey.UpArrow:
                    SelectPrev();
                    break;
                case ConsoleKey.RightArrow:
                    NextElement();
                    break;
                case ConsoleKey.LeftArrow:
                    BackElement();
                    break;
                case ConsoleKey.Enter:
                    SelectActionWithBranches();
                    read = false;
                    break;
                case ConsoleKey.X:
                    MarkElement();
                    break;
            }
        }

        void SelectNext()
        {
            BackStack.Peek().Content.SwitchColor();
            BackStack.Pop();

            selectedElementNumber++;
            if (selectedElementNumber > BackStack.Peek().Children.Count)
                selectedElementNumber = 1;

            BackStack.Push(BackStack.Peek().Children[selectedElementNumber - 1]);

            BackStack.Peek().Content.SwitchColor();
        }

        void SelectPrev()
        {
            BackStack.Peek().Content.SwitchColor();
            BackStack.Pop();
            if (selectedElementNumber < 2)
            {
                selectedElementNumber = BackStack.Peek().Children.Count;
            }
            else
            {
                selectedElementNumber--;
            }

            BackStack.Push(BackStack.Peek().Children[selectedElementNumber - 1]);

            BackStack.Peek().Content.SwitchColor();
        }

        bool NextElement()
        {
            if (BackStack.Peek().Children.Count <= 0) return false;

            BackStack.Peek().Content.SwitchColor();

            if (_exploreAll)
            {
                selectedElementNumber = 1;
                BackStack.Push(BackStack.Peek().Children.First());
            }
            else
            {
                var treeEnd = Console.CursorTop;

                Console.SetCursorPosition(0, treeStartPos);
                BackStack.Peek().PrintOneLayer(countOff);
                ClearLines(treeEnd);

                selectedElementNumber = 1;
                BackStack.Push(BackStack.Peek().Children.First());
            }

            BackStack.Peek().Content.SwitchColor();

            return true;
        }

        bool BackElement()
        {
            if (BackStack.Count <= 2) return false;

            BackStack.Peek().Content.SwitchColor();

            if (_exploreAll)
            {
                BackStack.Pop();
                var temp = BackStack.Pop();
                selectedElementNumber = BackStack.Peek().Children.IndexOf(temp) + 1;
                BackStack.Push(temp);
            }
            else
            {
                var treeEnd = Console.CursorTop;

                BackStack.Pop();
                var temp = BackStack.Pop();
                selectedElementNumber = BackStack.Peek().Children.IndexOf(temp) + 1;


                Console.SetCursorPosition(0, treeStartPos);
                BackStack.Peek().PrintOneLayer(countOff);
                ClearLines(treeEnd);

                BackStack.Push(temp);
            }

            BackStack.Peek().Content.SwitchColor();

            return true;
        }

        void MarkElement()
        {
            if (BackStack.Peek().Content.IsMarked)
            {
                BackStack.Peek().Content.IsMarked = false;
                _markedNods.Remove(BackStack.Last());
            }
            else
            {
                BackStack.Peek().Content.IsMarked = true;
                _markedNods.Add(BackStack.Peek());
            }


            var pos = Console.GetCursorPosition();
            BackStack.Peek().Content.PrintNameCount();
            Console.SetCursorPosition(pos.Left, pos.Top);
        }

        void ClearLines(int end)
        {
            var newTreeEnd = Console.CursorTop;
            while (Console.CursorTop < end)
                Console.WriteLine(new string(' ', Console.BufferWidth - 1));

            Console.SetCursorPosition(0, newTreeEnd);
        }
    }

    private void SelectActionWithBranches()
    {
        if (!_headersOff)
        {
            PrintHint();
        }

        void PrintHint()
        {
            Console.WriteLine(new string('_', Console.BufferWidth));
            Console.WriteLine("Select action with selected branches.\n");
            PrintHintButton("[Enter] - Just output to console(Don't exit)");
            Console.Write("   ");
            PrintHintButton("[A] - Append to file");
            Console.Write("\n\n");
            PrintHintButton("[S] - Save in txt file(rewrite)");
            Console.Write("   ");
            PrintHintButton("[Esc] - Exit");
            // Console.Write("   ");
            // PrintHintButton("[Backspace] - Back");  //TODO
            Console.Write("\n\n");
        }

        while (true)
        {
            var k = Console.ReadKey(true);

            if (k.Key == ConsoleKey.Escape) return;

            string? line;
            int count;
            switch (k.Key)
            {
                case ConsoleKey.Enter:
                    var result = GetUrisFromMarked(out count);
                    Console.WriteLine("Result " + count + " links:");
                    Console.WriteLine(result);
                    PrintHint();
                    break;
                case ConsoleKey.S:
                    Console.Write("Path:");
                    line = Console.ReadLine();
                    try
                    {
                        File.WriteAllText(line, GetUrisFromMarked(out count));
                        Console.WriteLine("Success. " + count + " links written.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e);
                    }

                    PrintHint();
                    break;
                case ConsoleKey.A:
                    Console.Write("Path:");
                    line = Console.ReadLine();
                    try
                    {
                        File.AppendAllText(line, '\n' + GetUrisFromMarked(out count));
                        Console.WriteLine("Success. " + count + " links written.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine();
                        Console.WriteLine(e);
                    }

                    PrintHint();
                    break;
            }
        }

        string GetUrisFromMarked(out int count)
        {
            List<NodeTree> result = new(Root.Content.Count);
            foreach (var node in _markedNods)
            {
                result.Add(node);
                AddChild(node);
            }

            void AddChild(NodeTree node)
            {
                foreach (var child in node.Children)
                {
                    if (!result.Contains(child))
                        result.Add(child);

                    AddChild(child);
                }
            }

            List<string> uris = new();

            foreach (var node in result)
                uris.AddRange(node.Content.Uris.Select(uri => uri.ToString()));

            count = uris.Count;
            return string.Join('\n', uris);
        }
    }
}