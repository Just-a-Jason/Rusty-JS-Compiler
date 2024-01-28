internal class U16 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.U16;
    public ushort Value { get; }

    public U16(ushort value) {
        Value = value;
    }
}

