internal class IdentifierNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.Identifier;
    public string Text;

    public IdentifierNode(string symbol) {
        this.Text = symbol;
    }

    public override string ToString() {
        return Text;
    }
}
