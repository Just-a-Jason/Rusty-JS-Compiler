internal class RustyInterpreter {

    public RuntimeValueTypeNode Evaluate(StatementNode node) {
        switch(node.Kind) {
            case NodeType.NumericLiteral:
                return GetValueType((NumericLiteralNode)node);
            default:
                return new Nil();
        }
    }

    private RuntimeValueTypeNode GetValueType(NumericLiteralNode node) {
        if (node.Value >= sbyte.MinValue && node.Value <= sbyte.MaxValue)
            return new I8((sbyte)node.Value);
        if (node.Value >= ushort.MinValue && node.Value <= ushort.MaxValue)
            return new I16((short)node.Value);
        if (node.Value >= int.MinValue && node.Value <= int.MaxValue)
            return new I32((int)node.Value);
        if (node.Value >= long.MinValue && node.Value <= long.MaxValue)
            return new I64((long)node.Value);

        RustyErrorHandler.Error($"Value: {node.Value} is too big to handle!", 1250);
        return null;
    }
}
