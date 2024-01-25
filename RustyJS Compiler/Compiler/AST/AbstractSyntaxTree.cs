internal class AbstractSyntaxTree {
    public AstNode Root { get; }

    public AbstractSyntaxTree(AstNode root) {
        this.Root = root;
    }

    public void ExecuteTree() { 
    }
}
