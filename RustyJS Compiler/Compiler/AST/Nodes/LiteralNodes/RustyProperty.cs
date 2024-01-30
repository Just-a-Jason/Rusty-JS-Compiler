internal class RustyProperty : ExpressionNode {
    public AccessModifier Modifier { get; }
    public override NodeType Kind { get; } = NodeType.RustyProperty;
    public ExpressionNode? Value { get; }
    public string Name { get; }

    public RustyProperty(string name, AccessModifier modifier,ExpressionNode? value= null) {
        Modifier = modifier;
        Value = value;
        Name = name;
    }

    public override string ToString() {
        if (Value == null) return $"this.{Name};";
        return $"this.{Name}={Value};";
    }
}
