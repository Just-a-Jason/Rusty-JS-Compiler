internal class NumericLiteralNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.NumericLiteral;
    public double Value { get; }

    public NumericLiteralNode(double value) {
        Value = value;
    }

    public override string ToString() {
        return Value.ToString();
    }
}
