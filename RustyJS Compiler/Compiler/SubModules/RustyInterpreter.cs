internal class RustyInterpreter {

    public RuntimeValueTypeNode Evaluate(StatementNode node) {
        switch(node.Kind) {
            case NodeType.NumericLiteral:
                return GetValueType((NumericLiteralNode)node);
            default:
                return new Nil();
        }
    }

    private RuntimeValueTypeNode? GetValueType(NumericLiteralNode node) {
        int decimals = CountDecimalPlaces(node.Value);
        if (CountDecimalPlaces(node.Value) > 0) {
            if (decimals <= F32.MaxDecimals) return new F32((float)node.Value);
            else if(decimals <= F64.MaxDecimals) return new F64((float)node.Value);
            RustyErrorHandler.Error($"Value: {node.Value} is too large for type (F32 or F64).", 1250);
        }
        else if (node.Value >= sbyte.MinValue && node.Value <= sbyte.MaxValue)
            return new I8((sbyte)node.Value);
        else if (node.Value >= ushort.MinValue && node.Value <= ushort.MaxValue)
            return new I16((short)node.Value);
        else if (node.Value >= int.MinValue && node.Value <= int.MaxValue)
            return new I32((int)node.Value);
        else if (node.Value >= long.MinValue && node.Value <= long.MaxValue)
            return new I64((long)node.Value);

        RustyErrorHandler.Error($"Value: {node.Value} is too large to handle!", 1250);
        return null;
    }

    static int CountDecimalPlaces(double value) {
        string stringValue = value.ToString();

        int decimalPointIndex = stringValue.IndexOf('.');

        if (decimalPointIndex == -1 || decimalPointIndex == stringValue.Length - 1) {
            return 0;
        }

        // Calculate the number of characters after the decimal point
        int decimalPlaces = stringValue.Length - 1 - decimalPointIndex;

        return decimalPlaces;
    }
}
