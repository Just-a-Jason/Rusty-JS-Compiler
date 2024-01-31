internal class AssignmentExpressionNode: ExpressionNode {
    public override NodeType Kind { get; } = NodeType.AssignmentExpression;
    public ExpressionNode Assigne { get; }
    public ExpressionNode Value { get; }

    public AssignmentExpressionNode(ExpressionNode assignment, ExpressionNode value) {
        Assigne = assignment;
        Value = value;
    }

    public override string ToString() => $"{Assigne}={Value};";
}
