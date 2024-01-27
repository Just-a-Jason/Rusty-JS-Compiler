internal class BinaryExpressionNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.BinaryExpression;
    public ExpressionNode Left { get; set; }
    public ExpressionNode Right { get; set; }
    public string Operator { get; set; }
}
