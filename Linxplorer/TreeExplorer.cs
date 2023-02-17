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
    private List<NodeTree> MarkedNods = new();

    public void Explore(bool exploreAll = false, bool headersOff = false, bool countOff = false)
    {
        if (!headersOff)
        {
            Console.Write('\n');
            PrintHintButton("[↑] - Up");
            Console.Write("   ");
            PrintHintButton("[↓] - Down");
            Console.Write("   ");
            PrintHintButton("[→] - Next element");
            Console.Write("   ");
            PrintHintButton("[←] - Previous element");
            Console.Write("   ");
            PrintHintButton("[Enter] - Print branch");
            Console.Write("   ");
            PrintHintButton("[Esc] - Exit");
            Console.Write("\n\n");
        }


        _exploreAll = exploreAll;
        _headersOff = headersOff;
        SelectorEnable(countOff);
    }

    private void SelectorEnable(bool countOff = false)
    {
        var treeStartPos = Console.CursorTop;
        Coincidence.CurrentWindowPos = treeStartPos - Coincidence.Padding;
        
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
            BackStack.Peek().Content.SwitchColor(countOff);
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
                    PrintBranchLinks();
                    read = false;
                    break;
            }
        }

        void PrintBranchLinks()
        {
            var printRoot = BackStack.Peek();

            if (!_headersOff)
            {
                Console.WriteLine('\n' + new string('_', Console.BufferWidth));

                foreach (var node in BackStack.Reverse())
                    Console.Write(node.Content.Name + '/');

                printRoot.Content.Print();
                Console.Write("\n\n");
            }

            Print(printRoot);

            void Print(NodeTree node)
            {
                if (node.Content.Uri != null)
                    Console.WriteLine(node.Content.Uri.ToString());

                if (node.Children.Count > 0)
                {
                    foreach (var child in node.Children)
                        Print(child);
                }
            }
        }

        void SelectNext()
        {
            BackStack.Peek().Content.SwitchColor(countOff);
            selectedElementNumber++;
            BackStack.Pop();

            if (selectedElementNumber > BackStack.Peek().Children.Count)
            {
                selectedElementNumber = 1;
                BackStack.Push(BackStack.Peek().Children[selectedElementNumber - 1]);
            }
            else
            {
                BackStack.Push(BackStack.Peek().Children[selectedElementNumber - 1]);
            }

            BackStack.Peek().Content.SwitchColor(countOff);
        }

        void SelectPrev()
        {
            BackStack.Peek().Content.SwitchColor(countOff);
            BackStack.Pop();
            if (selectedElementNumber < 2)
            {
                selectedElementNumber = BackStack.Peek().Children.Count;
            }
            else
            {
                selectedElementNumber--;
                BackStack.Push(BackStack.Peek().Children[selectedElementNumber - 1]);
            }

            BackStack.Peek().Content.SwitchColor(countOff);
        }

        bool NextElement()
        {
            if (BackStack.Peek().Children.Count <= 0) return false;

            BackStack.Peek().Content.SwitchColor(countOff);

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

            BackStack.Peek().Content.SwitchColor(countOff);

            return true;
        }

        bool BackElement()
        {
            if (BackStack.Count <= 2) return false;

            BackStack.Peek().Content.SwitchColor(countOff);

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

            BackStack.Peek().Content.SwitchColor(countOff);

            return true;
        }

        void ClearLines(int end)
        {
            var newTreeEnd = Console.CursorTop;
            while (Console.CursorTop < end)
                Console.WriteLine(new string(' ', Console.BufferWidth - 1));

            Console.SetCursorPosition(0, newTreeEnd);
        }
    }

    private void PrintHintButton(string hint)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(hint);
        Console.ResetColor();
    }
}