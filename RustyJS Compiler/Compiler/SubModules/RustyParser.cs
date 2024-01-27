using System.Globalization;

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
    }

    private StatementNode ParseStatement() {
        return ParseExpression();
    }

    private ExpressionNode ParsePrimaryExpression() {
        Token tk = CurrentToken();

        switch(tk.TokenType) {
            case TokenType.Identifier:
                return new IdentifierNode(ConsumeToken().Text);
            case TokenType.Number:
                return new NumericLiteralNode(double.Parse(ConsumeToken().Text, CultureInfo.InvariantCulture));
            default:
                RustyErrorHandler.Error($"Unexpected token: \"{ConsumeToken().Text}\"", 950);
                return new BinaryExpressionNode();
        }
    }

    private ExpressionNode ParseExpression() {
        return ParsePrimaryExpression();
    }

    private ProgramNode CreateProgramRootNode() {
        ProgramNode program = new ProgramNode();
        return program;
    }

    private bool EndOfFile() => CurrentToken().TokenType != TokenType.EOF;
    private Token CurrentToken() => _tokens.Peek();
    private Token ConsumeToken() => _tokens.Dequeue();
}

