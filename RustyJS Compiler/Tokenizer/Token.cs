namespace Compiler.Tokenizer.Token {
    internal struct Token {
        public TokenType TokenType { get;  }
        public string Text { get; }
    
        public long Line {  get; }
        public long Char {  get; }

        public Token(TokenType tokenType, string text, long character, long line) {
            this.TokenType = tokenType;
            this.Text = text;
            Char = character;
            Line = line;
        }
    }

}
