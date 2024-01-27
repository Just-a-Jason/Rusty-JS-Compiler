internal class IdentifierNode : ExpressionNode {
    public NodeType Kind = NodeType.Identifier;
    public string symbol;

    public IdentifierNode(string symbol) {
        this.symbol = symbol;
    }
}
