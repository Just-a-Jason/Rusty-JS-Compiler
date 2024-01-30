internal class IdentifierNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.Identifier;
    public string Text;

    public IdentifierNode(string text) {
        this.Text = text;
    }

    public override string ToString() => Text;
}
