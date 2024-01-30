internal class NullLiteralNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.NullLiteral;
    public string Value { get; } = "null";

    public override string ToString() => "null";
}
