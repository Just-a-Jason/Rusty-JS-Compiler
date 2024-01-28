internal class I16 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.I16;
    public short Value { get; }
    
    public I16(short value) {
        Value = value;
    }
}

