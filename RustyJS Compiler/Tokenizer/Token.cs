internal struct Token {
    public TokenType TokenType { get;  }
    public string Text { get; }

    public Token(TokenType tokenType, string text) {
        this.TokenType = tokenType;
        this.Text = text;
    }
}
