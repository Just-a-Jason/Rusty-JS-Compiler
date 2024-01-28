using System.Net.Http.Headers;

internal class NullLiteralNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.NullLiteral;
    public string Value { get; } = "null";
}
