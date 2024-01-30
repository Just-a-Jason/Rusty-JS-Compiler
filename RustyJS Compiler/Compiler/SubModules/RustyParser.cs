using System.Globalization;
using Windows.UI.WebUI;

internal class RustyParser {
    private Queue<Token> _tokens;
    
    public RustyParser(Queue<Token> tokens) {
        _tokens = tokens;    
    }

    public string ParseTokens() {
        string outJs = string.Empty;
        AbstractSyntaxTree tree = new AbstractSyntaxTree(CreateProgramRootNode());
        while (EndOfFile()) {
            StatementNode node = ParseStatement();
            outJs += node.ToString();
            tree.Root.Body.Add(node);
        }
        tree.VisualizeTree();

        RustyInterpreter interpreter = new RustyInterpreter();
        RustyEnvironment globalScope = new RustyEnvironment();
        
        RuntimeValueTypeNode result = interpreter.Evaluate(tree.Root, globalScope);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(result + " ");
        Console.ForegroundColor = ConsoleColor.Red;
        if(!(result is Nil)) {
            Console.WriteLine(RustyInterpreter.GetValue(result));
        }
        else Console.WriteLine(result);
        Console.ForegroundColor = ConsoleColor.White;
        return outJs;
    }

    private StatementNode ParseStatement() {
        switch(CurrentToken().TokenType) {
            // Variable declaration

            case TokenType.ConstantKeyWord:
            case TokenType.UmutKeyword:
            case TokenType.MutKeyword:
                return ParseVariableDeclaration();
            
            default:
                return ParseExpression();
        }
    }

    private StatementNode ParseVariableDeclaration() {
        TokenType tkt = ConsumeToken().TokenType;
        bool isConstant = false;
        bool isMuttable = true;

        switch(tkt) {
            case TokenType.UmutKeyword:
                isMuttable = false;
            break;
            case TokenType.MutKeyword:
                isMuttable = true;
                break;
            case TokenType.ConstantKeyWord:
                isMuttable = false;
                isConstant = true;
                break;
        }

        string variableName = ExpectToken(TokenType.Identifier, $"Variable name is excepted after mut | umut | const keyword!").Text;
        
        if (CurrentToken().TokenType == TokenType.Colon) {
            ConsumeToken();
            RuntimeValueTypeNode variableType = GetVariableType();

            Console.WriteLine("Variable type of variable named: {0} is {1}", variableName, variableType);
        }
        
        if (CurrentToken().TokenType == TokenType.Semi) {
            ConsumeToken();
            if(isConstant)  RustyErrorHandler.Error("The constant variable requires a value or expression!", 5000);

            return new VariableDeclarationNode(variableName, isConstant, isMuttable, null);
        }

        ExpectToken(TokenType.Equals, "\"=\" is required to assign a value to variable!");
        VariableDeclarationNode node =  new VariableDeclarationNode(variableName, isConstant, isMuttable, ParseExpression());

        ExpectToken(TokenType.Semi, "Expected semicolon \";\" token.");
        return node;
    }

    private ExpressionNode ParseAssignmentExpression() {
        ExpressionNode left = ParseObjectExpression();

        if (CurrentToken().TokenType == TokenType.Equals) {
            ConsumeToken();
            ExpressionNode value = ParseAssignmentExpression();
            return new AssignmentExpressionNode(left, value);
        }

        return left;
    }

    private ExpressionNode ParseExpression() {
        return ParseAssignmentExpression();
    }

    private ExpressionNode ParseAdditiveExpression() {
        ExpressionNode left = ParseMultiplictitiveExpression();

        while (CurrentToken().Text == "+" || CurrentToken().Text == "-") {
            string Operator = ConsumeToken().Text;
            ExpressionNode right = ParseMultiplictitiveExpression();

            left = new BinaryExpressionNode(left, right, Operator);
        }

        return left;
    }

    private ExpressionNode ParseMultiplictitiveExpression() {
        ExpressionNode left = ParsePrimaryExpression();

        while (CurrentToken().Text == "/" || CurrentToken().Text == "*" || CurrentToken().Text == "%") {
            string Operator = ConsumeToken().Text;
            ExpressionNode right = ParsePrimaryExpression();

            left = new BinaryExpressionNode(left, right, Operator);
        }

        return left;
    }
    private ExpressionNode ParsePrimaryExpression() {
        Token tk = CurrentToken();

        switch(tk.TokenType) {
            case TokenType.Identifier:
                return new IdentifierNode(ConsumeToken().Text);
            case TokenType.Number:
                return new NumericLiteralNode(double.Parse(ConsumeToken().Text, CultureInfo.InvariantCulture));
            case TokenType.Null:
                ConsumeToken();
                return new NullLiteralNode();
            case TokenType.OpenPrent:
                ConsumeToken();
                ExpressionNode value = ParseExpression();
                ExpectToken(TokenType.ClosePrent, "Closing parenthesis was expected.");
                return value;
            default:
                RustyErrorHandler.Error($"Unexpected token: \"{ConsumeToken().Text}\"", 950);
                return null;
        }
    }

    private ExpressionNode ParseObjectExpression() {
        if (CurrentToken().TokenType != TokenType.ClassKeyword)
            return ParseAdditiveExpression();
        ConsumeToken();
        string parentClassName = ExpectToken(TokenType.Identifier, "A class name is required after class keyword.").Text;

        IdentifierNode? extends=null; 
        
        if(CurrentToken().TokenType == TokenType.Colon) {
            ConsumeToken();
            string extendIdent = ExpectToken(TokenType.Identifier, $"A class name for extended class is required! After \":\" ").Text;
            extends = new IdentifierNode(extendIdent);
        }
        
        List<RustyProperty> props = new List<RustyProperty>();
        while (EndOfFile() && CurrentToken().TokenType != TokenType.EndKeyWord) {
            if(CurrentToken().TokenType == TokenType.ClassKeyword) {
                Token tk = ConsumeToken();
                string className = ExpectToken(TokenType.Identifier, "A class name is required after class keyword.").Text;

                RustyErrorHandler.Error($"Cannot declare another class inside class. \"{parentClassName}\" > \"{className}\" (line: {tk.Line}, chr: {tk.Char})", 7000);
            }
            if (IsAccessModifier()) {
                // Property Declaration logic
            }
            ConsumeToken();
        }
        ExpectToken(TokenType.EndKeyWord, "Expected \"end\"  keyword after class definition.");
        return new ObjectLiteralNode(props, parentClassName, extends);
    }

    private ProgramNode CreateProgramRootNode() => new ProgramNode();

    private Token ExpectToken(TokenType type, string errorMsg) {
        Token tk = ConsumeToken();
        if (tk.TokenType != type) RustyErrorHandler.Error(errorMsg + $" (line: {tk.Line}, chr: {tk.Char})", 1110);
        return tk;
    }

    private bool EndOfFile() => CurrentToken().TokenType != TokenType.EOF;
    private Token ConsumeToken() => _tokens.Dequeue();
    private Token CurrentToken() => _tokens.Peek();

    private RuntimeValueTypeNode GetVariableType() {
        Token tk = ConsumeToken();

        switch (tk.TokenType) {
            case TokenType.UInt8:
                return new U8();
            case TokenType.UInt16:
                return new U16();
            case TokenType.UInt32:
                return new U32();
            case TokenType.UInt64:
                return new U64();
            case TokenType.Int8:
                return new I8();
            case TokenType.Int16:
                return new I16();
            case TokenType.Int32:
                return new I32();
            case TokenType.Int64:
                return new I64();
            case TokenType.Float32:
                return new F32();
            case TokenType.Float64:
                return new F64();

            case TokenType.Bool:
                return new Bool();

            default:
                RustyErrorHandler.Error($"Expected variable annotation type token.", 6300);
                return null;
        }
    }

    private bool IsAccessModifier() => CurrentToken().TokenType == TokenType.PublicKeyword || CurrentToken().TokenType == TokenType.PrivateKeyword || CurrentToken().TokenType == TokenType.ProtectedKeyword; 
}

