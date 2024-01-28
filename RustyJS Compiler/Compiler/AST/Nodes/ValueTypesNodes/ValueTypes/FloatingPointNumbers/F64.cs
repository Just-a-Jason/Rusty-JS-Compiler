internal class F64 : RuntimeValueTypeNode {
    public static sbyte MaxDecimals { get; } = 17;
    public override ValueType Type { get; } = ValueType.F64;
    public double Value { get; }

    public F64(double value=0d) { 
        Value = value;
    }
}

