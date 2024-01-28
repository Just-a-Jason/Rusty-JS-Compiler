internal class IdentifierNode : ExpressionNode {
    public NodeType Kind = NodeType.Identifier;
    public string Symbol;

    public IdentifierNode(string symbol) {
        this.Symbol = symbol;
    }
}
