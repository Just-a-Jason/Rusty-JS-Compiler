﻿using System.Globalization;

internal class RustyParser {
    private Queue<Token> _tokens;
    
    public RustyParser(Queue<Token> tokens) {
        _tokens = tokens;    
    }

    public void ParseTokens() {
        AbstractSyntaxTree tree = new AbstractSyntaxTree(CreateProgramRootNode());
        while (EndOfFile()) {
            tree.Root.Body.Add(ParseStatement());
        }
        tree.VisualizeTree();

        RustyInterpreter interpreter = new RustyInterpreter();
        RustyEnvironment globalScope = new RustyEnvironment(null);
        
        RuntimeValueTypeNode result = interpreter.Evaluate(tree.Root, globalScope);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(result + " ");
        Console.ForegroundColor = ConsoleColor.Red;
        if(!(result is Nil)) {
            Console.WriteLine(RustyInterpreter.GetValue(result));
        }
        else Console.WriteLine(result);
        Console.ForegroundColor = ConsoleColor.White;
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
                isConstant = true;
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

    private ExpressionNode ParseExpression() {
        return ParseAdditiveExpression();
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

    private ProgramNode CreateProgramRootNode() => new ProgramNode();

    private Token ExpectToken(TokenType type, string errorMsg) {
        Token tk = ConsumeToken();
        if (tk.TokenType != type) RustyErrorHandler.Error(errorMsg, 1110);
        return tk;
    }

    private bool EndOfFile() => CurrentToken().TokenType != TokenType.EOF;
    private Token ConsumeToken() => _tokens.Dequeue();
    private Token CurrentToken() => _tokens.Peek();
}

