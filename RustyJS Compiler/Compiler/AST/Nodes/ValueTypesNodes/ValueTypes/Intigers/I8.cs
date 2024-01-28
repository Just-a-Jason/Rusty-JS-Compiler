internal class I8 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.I8;
    public sbyte Value { get; }

    public I8(sbyte value) {
        Value = value;
    }
}

