internal class RustyInterpreter {
    public RuntimeValueTypeNode Evaluate(StatementNode node, RustyEnvironment env) {
        switch(node.Kind) {
            case NodeType.NumericLiteral:
                return new F64(((NumericLiteralNode)node).Value);
            case NodeType.NullLiteral:
                return new Nil();
            case NodeType.BinaryExpression:
                return EvaluateBinaryExpression((BinaryExpressionNode)node, env);
            case NodeType.Identifier:
                return EvaluateIdentifier((IdentifierNode)node, env);
            case NodeType.Program:
                return EvaluateProgram((ProgramNode)node, env);

            case NodeType.VariableDeclaration:
                return EvaluateVariableDeclaration((VariableDeclarationNode)node, env);
            
            default:
                RustyErrorHandler.Error($"{node} is not setup for interpreter.", 2500);
                    return null;
        }
    }


    private RuntimeValueTypeNode EvaluateVariableDeclaration(VariableDeclarationNode declaration, RustyEnvironment env) {
        RuntimeValueTypeNode val = (declaration.Value == null) ? Evaluate(declaration.Value, env) : new Nil();
        
        return env.DeclareVariableInScope(declaration.VarName, val, declaration.IsMutable, declaration.IsConstant);
    }

    private RuntimeValueTypeNode EvaluateIdentifier(IdentifierNode ident, RustyEnvironment env) {
        RuntimeValueTypeNode value = env.GetVariable(ident.Symbol);
        return value;
    }

    private RuntimeValueTypeNode EvaluateProgram(ProgramNode program, RustyEnvironment env) {
        RuntimeValueTypeNode lastEvaluated = new Nil();

        foreach (StatementNode statement in program.Body) {
            lastEvaluated = Evaluate(statement, env);
        }

        if (lastEvaluated is Nil) return lastEvaluated;

        return GetValueType(GetValue(lastEvaluated));
    }
        
    private RuntimeValueTypeNode EvaluateNumericBinaryExpression(RuntimeValueTypeNode rhs, RuntimeValueTypeNode lhs, string Operator){
        double result = 0;
        
        double r = ((F64)rhs).Value;
        double l = ((F64)lhs).Value;

        switch (Operator) {
            case "+":
                result = l + r; 
            break;
            case "-":
                result = r - l; 
            break;
            case "*":
                result = r * l; 
            break;
            case "/":
                if(l == 0) RustyErrorHandler.Error($"Divide by zero exception accured.", 2645);
                result = l / r; 
                break;
            case "%":
                result = r % l;
                break;
        }
        return new F64(result);
    }

    private RuntimeValueTypeNode EvaluateBinaryExpression(BinaryExpressionNode node, RustyEnvironment env) {
        RuntimeValueTypeNode leftHandSide = Evaluate(node.Left, env);
        RuntimeValueTypeNode rightHandSide = Evaluate(node.Right, env);

        if (IsNumber(leftHandSide) && IsNumber(rightHandSide)) {
            return EvaluateNumericBinaryExpression(leftHandSide, rightHandSide, node.Operator);
        }

        return new Nil();
    }

    private bool IsNumber(RuntimeValueTypeNode node) {
        return
            node.Type == ValueType.I8 || node.Type == ValueType.I16
            || node.Type == ValueType.I32 || node.Type == ValueType.I64
            || node.Type == ValueType.F32 || node.Type == ValueType.F64
            || node.Type == ValueType.U8 || node.Type == ValueType.U16
            || node.Type == ValueType.U32 || node.Type == ValueType.U64;
    }

    public static double GetValue(RuntimeValueTypeNode node) {
        switch(node.Type) {
            case ValueType.Boolean:
                return Convert.ToInt32(((Bool)node).Value);
            case ValueType.I8:
                return ((I8)node).Value; 
            case ValueType.I16:
                return ((I16)node).Value;
            case ValueType.I32:
                return ((I32)node).Value;
            case ValueType.I64:
                return ((I64)node).Value;
            case ValueType.U8:
                return ((U8)node).Value;
            case ValueType.U16:
                return ((U16)node).Value;
            case ValueType.U32:
                return ((U32)node).Value;
            case ValueType.U64:
                return ((U64)node).Value;
            case ValueType.F32:
                return ((F32)node).Value;
            default:
                return ((F64)node).Value;
        }
    }

    public static RuntimeValueTypeNode GetValueType(NumericLiteralNode node) {
        return GetValueType(node.Value);
    }

    public static RuntimeValueTypeNode GetValueType(double value) {
        int decimals = CountDecimalPlaces(value);
        if (decimals > 0)
        {
            if (decimals <= F32.MaxDecimals) return new F32((float)Math.Round(value, decimals));
            else if (decimals <= F64.MaxDecimals) return new F64(Math.Round(value, decimals));
            RustyErrorHandler.Error($"Value: {value} is too large for type (F32 or F64).", 1250);
        }
        else if (value == 1 || value == 0)
            return new Bool(Convert.ToBoolean(value));
        else if (value >= 0 && value <= byte.MaxValue)
            return new U8((byte)value);
        else if (value >= 0 && value <= ushort.MaxValue)
            return new U16((ushort)value);
        else if (value >= 0 && value <= uint.MaxValue)
            return new U32((uint)value);
        else if (value >= 0 && value <= ulong.MaxValue)
            return new U64((uint)value);
        else if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            return new I8((sbyte)value);
        else if (value >= short.MinValue && value <= short.MaxValue)
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

        int decimalPointIndex = stringValue.IndexOf(',');

        if (decimalPointIndex == -1 || decimalPointIndex == stringValue.Length - 1) {
            return 0;
        }

        int decimalPlaces = stringValue.Length - 1 - decimalPointIndex;

        return decimalPlaces;
    }
}
