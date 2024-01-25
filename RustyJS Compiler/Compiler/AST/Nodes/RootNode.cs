internal class RootNode : AstNode {
    private List<AstNode> _children = new List<AstNode>();

    public RootNode(string name) : base(name) {
    }

    public void AddChildNode(AstNode child) => this._children.Add(child);
}

