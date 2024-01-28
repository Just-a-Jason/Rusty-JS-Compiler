internal class Bool : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.Boolean;
    public bool Value { get; }
    
    public Bool(bool value) {
        Value = value;
    }
}

