internal class Tokenizer {

    private Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>() {
        { "umut", TokenType.UmutKeyword },
        { "mut", TokenType.MutKeyword },
        { "fn", TokenType.FunctionKeyWord },
        { "end", TokenType.EndKeyWord },
        { "namespace", TokenType.NameSpaceKeyWord },
        { "ret", TokenType.ReturnKeyWord },
        { "true", TokenType.BooleanValue },
        { "false", TokenType.BooleanValue },
        { "const", TokenType.ConstantKeyWord },
        { "nil", TokenType.Null },
        { "i8",  TokenType.Int8 },
        { "i16",  TokenType.Int16 },
        { "i32",  TokenType.Int32 },
        { "i64",  TokenType.Int64 },
        { "u8",  TokenType.UInt8 },
        { "u16",  TokenType.UInt16 },
        { "u32",  TokenType.UInt32 },
        { "u64",  TokenType.UInt64 },
        { "F32",  TokenType.Float32 },
        { "F64",  TokenType.Float64 },
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
        return _chars.Dequeue();
    }
}