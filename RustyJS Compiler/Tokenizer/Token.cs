internal struct Token {
    public TokenType tokenType;
    public string text;

    public Token(TokenType tokenType, string text) {
        this.tokenType = tokenType;
        this.text = text;
    }
}
