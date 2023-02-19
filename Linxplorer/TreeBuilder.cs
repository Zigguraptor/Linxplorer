namespace Linxplorer;

public static class TreeBuilder
{
    public static NodeTree BuildFilesTree(AnalysisResult analysisResult)
    {
        var tree = new NodeTree(new Coincidence("Files", true));

        if (analysisResult.ParsedFiles.Length > 1)
        {
            foreach (var parsedFile in analysisResult.ParsedFiles)
            {
                var branch = BuildLinksTree(parsedFile.Links);
                branch.Content.Name = parsedFile.FileName;
                branch.Content.Count = parsedFile.Links.Length;
                tree.Children.Add(branch);
            }
        }
        else
        {
            foreach (var parsedFile in analysisResult.ParsedFiles)
            {
                tree = BuildLinksTree(parsedFile.Links);
                tree.Content.Name = parsedFile.FileName;
                tree.Content.Count = parsedFile.Links.Length;
            }
        }

        return tree;
    }

    public static NodeTree BuildLinksTree(Uri[] uris)
    {
        var root = new NodeTree(new Coincidence("Links", 0));
        NodeTree currentNode;

        foreach (var uri in uris)
        {
            var domain = uri.Host;
            if (domain == "") domain = "NO DOMAIN";
            var temp = uri.AbsolutePath.Split("/");
            var path = temp[^1] == "" ? new string[temp.Length - 2] : new string[temp.Length - 1];
            Array.Copy(temp, 1, path, 0, path.Length);

            FillDomain();

            void FillDomain()
            {
                foreach (var nodeChild in root.Children)
                {
                    if (nodeChild.Content.Name != domain) continue;

                    nodeChild.Content.Count++;
                    currentNode = nodeChild;
                    return;
                }

                currentNode = path.Length < 1
                    ? new NodeTree(new Coincidence(domain, 1, uri))
                    : new NodeTree(new Coincidence(domain, 1));

                root.Children.Add(currentNode);
            }

            for (var i = 0; i < path.Length; i++)
            {
                Find();

                void Find()
                {
                    foreach (var node in currentNode.Children)
                    {
                        if (node.Content.Name != path[i]) continue;

                        node.Content.Count++;
                        currentNode = node;
                        if (i == path.Length - 1)
                            node.Content.Uris.Add(uri);
                        return;
                    }

                    var newNode = i == path.Length - 1
                        ? new NodeTree(new Coincidence(path[i], 1, uri))
                        : new NodeTree(new Coincidence(path[i], 1));

                    currentNode.Children.Add(newNode);
                    currentNode = newNode;
                }
            }
        }

        return root;
    }
}