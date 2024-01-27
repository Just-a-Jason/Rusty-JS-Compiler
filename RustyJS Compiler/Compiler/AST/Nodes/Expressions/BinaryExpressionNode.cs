internal class BinaryExpressionNode : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.BinaryExpression;
    public ExpressionNode Left { get; }
    public ExpressionNode Right { get; }
    public string Operator { get; }

    public BinaryExpressionNode(ExpressionNode left, ExpressionNode right, string Operator) {
        this.Operator = Operator;
        Right = right;
        Left = left;
    }
}
