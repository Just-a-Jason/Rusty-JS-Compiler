using System.Globalization;

internal class RustyInterpreter {
    public RuntimeValueTypeNode Evaluate(StatementNode node) {
        switch(node.Kind) {
            case NodeType.NumericLiteral:
                return GetValueType((NumericLiteralNode)node);
            case NodeType.NullLiteral:
                return new Nil();
            case NodeType.BinaryExpression:
                return EvaluateBinaryExpression((BinaryExpressionNode)node);
            case NodeType.Program:
                return EvaluateProgram((ProgramNode)node);
            default:
                RustyErrorHandler.Error($"{node} is not setup for interpreter.", 2500);
                    return null;
        }
    }

    
    private RuntimeValueTypeNode EvaluateProgram(ProgramNode program) {
        RuntimeValueTypeNode lastEvaluated = new Nil();

        foreach (StatementNode statement in program.Body) {
            lastEvaluated = Evaluate(statement);
        }

        return lastEvaluated;
    }
        
    private RuntimeValueTypeNode EvaluateNumericBinaryExpression(RuntimeValueTypeNode rhs, RuntimeValueTypeNode lhs, string Operator){
        double result = 0;
        switch (Operator) {
            case "+":
                result = GetValue(lhs) + GetValue(rhs); 
            break;
            case "-":
                result = GetValue(rhs) - GetValue(rhs); 
            break;
            case "*":
                result = GetValue(rhs) * GetValue(lhs); 
            break;
            case "/":
                try { result = GetValue(lhs) / GetValue(lhs); }
                catch(DivideByZeroException) { RustyErrorHandler.Error($"Divide by zero excetion accured.", 2645); }
                break;
            case "%":
                result = GetValue(lhs) % GetValue(rhs);
                break;
        }
        return GetValueType(result);
    }
    private RuntimeValueTypeNode EvaluateBinaryExpression(BinaryExpressionNode node) {
        RuntimeValueTypeNode leftHandSide = Evaluate(node.Left);
        RuntimeValueTypeNode rightHandSide = Evaluate(node.Right);

        if (IsNumber(leftHandSide) && IsNumber(rightHandSide)) {
            return EvaluateNumericBinaryExpression(leftHandSide, rightHandSide, node.Operator);
        }

        return new Nil();
    }

    private bool IsNumber(RuntimeValueTypeNode node) {
        return
            node.Type == ValueType.I8 || node.Type == ValueType.I16
            || node.Type == ValueType.I32 || node.Type == ValueType.I64
            || node.Type == ValueType.I32 || node.Type == ValueType.F64;
    }

    private double GetValue(RuntimeValueTypeNode node) {
        switch(node.Type) {
            case ValueType.I8:
                return ((I8)node).Value; 
            case ValueType.I16:
                return ((I16)node).Value;
            case ValueType.I32:
                return ((I32)node).Value;
            case ValueType.I64:
                return ((I64)node).Value;
            case ValueType.F64:
                return ((F32)node).Value;
            default:
                return ((F64)node).Value;
        }
    }

    private RuntimeValueTypeNode GetValueType(NumericLiteralNode node) {
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

    private RuntimeValueTypeNode GetValueType(double value) {
        int decimals = CountDecimalPlaces(value);
        if (CountDecimalPlaces(value) > 0) {
            if (decimals <= F32.MaxDecimals) return new F32((float)value);
            else if (decimals <= F64.MaxDecimals) return new F64((float)value);
            RustyErrorHandler.Error($"Value: {value} is too large for type (F32 or F64).", 1250);
        }
        else if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            return new I8((sbyte)value);
        else if (value >= ushort.MinValue && value <= ushort.MaxValue)
            return new I16((short)value);
        else if (value >= int.MinValue && value <= int.MaxValue)
            return new I32((int)value);
        else if (value >= long.MinValue && value <= long.MaxValue)
            return new I64((long)value);

        RustyErrorHandler.Error($"Value: {value} is too large to handle!", 1250);
        return null;
    }

    static int CountDecimalPlaces(double value) {
        string stringValue = value.ToString();

        int decimalPointIndex = stringValue.IndexOf('.');

        if (decimalPointIndex == -1 || decimalPointIndex == stringValue.Length - 1) {
            return 0;
        }

        int decimalPlaces = stringValue.Length - 1 - decimalPointIndex;

        return decimalPlaces;
    }
}
