internal class Nil : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.Null;
    public string Value { get; } = "null";
}

