namespace Linxplorer;

public class NodeTree
{
    private const string Cross = " ├─";
    private const string Corner = " └─";
    private const string Vertical = " │ ";
    private const string Space = "   ";

    private string _name = "";
    private List<NodeTree> _children = new();

    public string Name
    {
        get
        {
            var name = Content?.ToString();
            if (name != null)
                _name = name;

            return _name;
        }
    }

    public List<NodeTree> Children
    {
        get => _children;
        set => _children = value;
    }

    public Coincidence Content { get; }

    public NodeTree(Coincidence content)
    {
        Content = content;
    }

    public void PrintNodeTree(bool countOff)
    {
        PrintNode(this, "");

        void PrintNode(NodeTree treeNode, string indent)
        {
            treeNode.Content.Print(countOff);
            Console.WriteLine(); // treeNode.Name

            var numberOfChildren = treeNode.Children.Count;

            for (var i = 0; i < numberOfChildren; i++)
                PrintChildNode(treeNode.Children[i], indent, (i == (numberOfChildren - 1)));
        }

        void PrintChildNode(NodeTree treeNode, string indent, bool isLast)
        {
            Console.Write(indent);

            if (isLast)
            {
                Console.Write(Corner);
                indent += Space;
            }
            else
            {
                Console.Write(Cross);
                indent += Vertical;
            }

            PrintNode(treeNode, indent);
        }
    }

    public void PrintOneLayer(bool countOff)
    {
        Content.Print(countOff);
        Console.WriteLine();

        var numberOfChildren = this.Children.Count;

        for (var i = 0; i < numberOfChildren; i++)
            PrintChildNode(this.Children[i], (i == (numberOfChildren - 1)));

        void PrintChildNode(NodeTree treeNode, bool isLast)
        {
            Console.Write(isLast ? Corner : Cross);

            treeNode.Content.Print(countOff);
            Console.WriteLine();
        }
    }

    public void SortByCount()
    {
        Children.Sort((x, y) => y.Content.Count - x.Content.Count);
        foreach (var child in Children)
            child.SortByCount();
    }

    // public void PrintOneLayer(string startPath)
    // {
    //     Console.WriteLine(startPath);
    //
    //     var numberOfChildren = Children.Count;
    //
    //     for (var i = 0; i < numberOfChildren; i++)
    //         PrintChildNode(Children[i], (i == (numberOfChildren - 1)));
    //
    //     void PrintChildNode(NodeTree treeNode, bool isLast)
    //     {
    //         Console.Write(isLast ? Corner : Cross);
    //
    //         Console.Write(startPath);
    //         Content.Print();
    //         Console.WriteLine();
    //     }
    // }
}