class ProgramNode : StatementNode {
    public override NodeType Kind { get; } = NodeType.Program;
    public List<StatementNode> Body { get; } = new List<StatementNode>();
}

