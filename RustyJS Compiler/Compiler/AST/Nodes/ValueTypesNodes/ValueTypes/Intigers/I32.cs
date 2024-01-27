internal class I32 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.I32;
    public int Value { get; }

    public I32(int value) {
        Value = value;
    }
}

