internal class IdentifierNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.Identifier;
    public string Symbol;

    public IdentifierNode(string symbol) {
        this.Symbol = symbol;
    }
}
