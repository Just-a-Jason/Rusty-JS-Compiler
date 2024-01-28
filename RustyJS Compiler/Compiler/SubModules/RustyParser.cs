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
        return ParseExpression();
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
                Expect(TokenType.ClosePrent, "Closing parenthesis was expected.");
                return value;
            default:
                RustyErrorHandler.Error($"Unexpected token: \"{ConsumeToken().Text}\"", 950);
                return null;
        }
    }

    private ProgramNode CreateProgramRootNode() => new ProgramNode();

    private Token Expect(TokenType type, string errorMsg) {
        Token tk = ConsumeToken();
        if (tk.TokenType != type) RustyErrorHandler.Error(errorMsg, 1110);
        return tk;
    }

    private bool EndOfFile() => CurrentToken().TokenType != TokenType.EOF;
    private Token ConsumeToken() => _tokens.Dequeue();
    private Token CurrentToken() => _tokens.Peek();
}

