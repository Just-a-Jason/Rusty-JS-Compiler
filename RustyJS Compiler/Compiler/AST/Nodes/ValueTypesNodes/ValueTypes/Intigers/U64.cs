internal class U64 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.U64;
    public ulong Value { get; }

    public U64(ulong value=0) {
        Value = value;
    }
}

