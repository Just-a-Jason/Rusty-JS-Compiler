internal class F32 : RuntimeValueTypeNode {
    public static sbyte MaxDecimals { get; } = 7;
    public override ValueType Type { get; } = ValueType.F32;
    public float Value { get; }

    public F32(float value=0f) {
        Value = value;
    }
}

