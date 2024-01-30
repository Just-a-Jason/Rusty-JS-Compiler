class VariableDeclarationNode : StatementNode {
    public override NodeType Kind { get; } = NodeType.VariableDeclaration;
    public RuntimeValueTypeNode ValueType { get; }
    public ExpressionNode? Value { get; }
    public bool IsConstant { get; }
    public bool IsStatic { get; }
    public bool IsMutable { get; }
    public string VarName { get; }

    public VariableDeclarationNode(string varname, bool isConstant, bool isMuttable, ExpressionNode? value) {
        IsConstant = isConstant;
        IsMutable = isMuttable;
        VarName = varname;
        Value = value;
    }

    public override string ToString() {
        string modifier = "var";

        if (IsConstant) modifier = "const";
        else if (!IsMutable) modifier = "let";
        string assignment = (Value != null) ? $"={Value};" : ";"; 
        return $"{modifier} {VarName}{assignment}";
    }
}

