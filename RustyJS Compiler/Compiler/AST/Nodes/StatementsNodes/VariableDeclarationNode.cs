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
}

