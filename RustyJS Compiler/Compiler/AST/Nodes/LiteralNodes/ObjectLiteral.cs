internal class ObjectLiteral : ExpressionNode {
    public List<RustyProperty> Properties { get; } = new List<RustyProperty>();
    public override NodeType Kind { get; } = NodeType.NullLiteral;

}
