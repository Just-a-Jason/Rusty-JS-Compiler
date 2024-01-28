internal class U32 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.U8;
    public uint Value { get; }

    public U32(uint value=0) {
        Value = value;
    }
}

