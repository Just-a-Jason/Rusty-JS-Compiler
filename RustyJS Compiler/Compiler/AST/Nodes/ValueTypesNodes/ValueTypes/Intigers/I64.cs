internal  class I64 : RuntimeValueTypeNode {
    public override ValueType Type { get; } = ValueType.I64;
    public long Value { get; }

    public I64(long value) { 
        Value = value;
    }
    
}

