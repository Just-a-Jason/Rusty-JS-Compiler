internal class U8 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.U8;
    public byte Value { get; }

    public U8(byte value=0) {
        Value = value;
    }
}

