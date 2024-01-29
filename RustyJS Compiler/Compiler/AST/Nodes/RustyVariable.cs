internal class RustyVariable {
    public RuntimeValueTypeNode ValueType { get; }
    public RuntimeValueTypeNode Value { get; set; }
    public bool Constant { get; }
    public bool Muttable { get; }


    public RustyVariable(bool constant, bool muttable, RuntimeValueTypeNode value) {
        Constant = constant;
        Muttable = muttable;
        Value = value;
    }

}

