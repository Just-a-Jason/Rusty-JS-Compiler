internal class Tokenizer {

    private Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>() {
        { "umut", TokenType.UmutKeyword },
        { "mut", TokenType.MutKeyword },
        { "fn", TokenType.FunctionKeyWord },
        { "end", TokenType.EndKeyWord },
        { "namespace", TokenType.NameSpaceKeyWord },
        { "ret", TokenType.ReturnKeyWord },
        { "rustyCompiler", TokenType.CompilerRuleSet },
        { "true", TokenType.BooleanValue },
        { "false", TokenType.BooleanValue },
        { "const", TokenType.ConstantKeyWord }
    };

    private Queue<char>? _chars;
    private long _line = 1;
    private long _pos = 0;

    public Queue<Token> Tokenize(string input) {

        Queue<Token> tokens = new Queue<Token>();
        _chars = new Queue<char>(input.ToCharArray());

        while (_chars.Count > 0) {

            char chr = _chars.Peek(); 
            
            if(chr == '\n') _line++;

            switch (chr) {
                case '(':
                    tokens.Enqueue(Token(TokenType.OpenPrent, chr));
                    Consume();
                    continue;
                case ')':
                    tokens.Enqueue(Token(TokenType.ClosePrent, chr));
                    Consume();
                    continue;
                case '*':
                case '+':
                case '-':
                case '/':
                case '%':
                    tokens.Enqueue(Token(TokenType.BinaryOperator, chr));
                    Consume();
                    continue;
                case '=':
                    tokens.Enqueue(Token(TokenType.Equals, chr));
                    Consume();
                    continue;
                case ';':
                    tokens.Enqueue(Token(TokenType.Semi, chr));
                    Consume();
                    continue;
            }

            if (char.IsDigit(chr)) {
                string number = string.Empty;

                while (_chars.Count > 0 && (char.IsDigit(_chars.Peek()) || _chars.Peek() == '.')) number += Consume();
                
                tokens.Enqueue(Token(TokenType.Number, number));
            }

            else if (char.IsLetter(chr)) {
                string identifier = string.Empty;

                while (_chars.Count > 0 && char.IsLetter(_chars.Peek())) identifier += Consume();

                if (KEYWORDS.TryGetValue(identifier, out TokenType reservedKeyWord))
                    tokens.Enqueue(Token(reservedKeyWord, identifier));
                else tokens.Enqueue(Token(TokenType.Identifier, identifier));
            }

            else if (IsSkippable(chr)) { Consume(); continue;}
            else RustyErrorHandler.Error($"Unrecognized character \"{chr}\" on position: (chr: {_pos}, line: {_line})", 550);
        }

        tokens.Enqueue(Token(TokenType.EOF, "EndOfFile"));
        return tokens;
    }

    private bool IsSkippable(char z) => (z == ' ' || z == '\n' || z == '\t' || z == '\r'); 
    private Token Token(TokenType type, string value) => new Token(type, value);
    private Token Token(TokenType type, char value) => new Token(type, value.ToString());
    private char Consume() {
        _pos++;
        return  _chars.Dequeue() ;
    }
}