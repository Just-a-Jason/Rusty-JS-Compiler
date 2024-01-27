internal class AbstractSyntaxTree {
    public ProgramNode Root { get; }

    public AbstractSyntaxTree(ProgramNode root) {
        Root = root;
    }

    public void VisualizeTree() {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{Root.Kind} (Root)");
        
        foreach (StatementNode node in Root.Body) {
        Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("   ╚ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(node);
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}
