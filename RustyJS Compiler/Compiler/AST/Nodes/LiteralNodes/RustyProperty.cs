internal class RustyProperty : ExpressionNode {
    public override NodeType Kind { get; } = NodeType.RustyProperty;
    public ExpressionNode? Value { get; }
    public string Key { get; }

    public RustyProperty(string key, ExpressionNode? value= null) {
        Value = value;
        Key = key;
    }
}
