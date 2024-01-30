using Windows.Security.Cryptography.Core;

internal class RustyTokenizer {

    private Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>() {
        { "umut", TokenType.UmutKeyword },
        { "mut", TokenType.MutKeyword },
        { "fn", TokenType.FunctionKeyWord },
        { "end", TokenType.EndKeyWord },
        { "namespace", TokenType.NameSpaceKeyWord },
        { "class", TokenType.ClassKeyword },
        { "pub", TokenType.PublicKeyword },
        { "priv", TokenType.PrivateKeyword },
        { "prot", TokenType.ProtectedKeyword },
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
        { "str", TokenType.String },
        { "chr", TokenType.Char },
        { "bool", TokenType.Bool },
        { "auto", TokenType.Auto },
        { "obj", TokenType.Object },
        { "arr", TokenType.Array }
    };

    private Queue<Token> _tokens;
    private Queue<char>? _chars;
    private long _line = 1;
    private long _pos = 0;

    public Queue<Token> Tokenize(string input) {
        _tokens = new Queue<Token>();
        _chars = new Queue<char>(input.ToCharArray());

        while (_chars.Count > 0) {

            char chr = _chars.Peek();

            if (chr == '\n') _line++;

            switch (chr) {
                case '{':
                    PushToken(TokenType.OpenBrace, chr);
                    ConsumeCharacter();
                    continue;
                case ',':
                    PushToken(TokenType.Comma, chr);
                    ConsumeCharacter();
                    continue;
                case '}':
                    PushToken(TokenType.CloseBrace, chr);
                    ConsumeCharacter();
                    continue;
                case '(':
                    PushToken(TokenType.OpenPrent, chr);
                    ConsumeCharacter();
                    continue;
                case ')':
                    PushToken(TokenType.ClosePrent, chr);
                    ConsumeCharacter();
                    continue;
                case '*':
                case '+':
                case '-':
                case '/':
                case '%':
                    PushToken(TokenType.BinaryOperator, chr);
                    ConsumeCharacter();
                    continue;
                case '=':
                    PushToken(TokenType.Equals, chr);
                    ConsumeCharacter();
                    continue;
                case ';':
                    PushToken(TokenType.Semi, chr);
                    ConsumeCharacter();
                    continue;
                case ':':
                    PushToken(TokenType.Colon, chr);
                    ConsumeCharacter();
                    if (IsQueueEmpty() && NextChar() == 'i' || NextChar() == 'f' || NextChar() == 'u' || NextChar() == 's' || NextChar() == 'c' || NextChar() == 'b' || NextChar() == 'o' || NextChar() == 'a') {
                        string typeToken = ConsumeCharacter().ToString();

                        // const b:bool = true;
                        // mut a:i32 = 10;
                        // umut a:i32;
                        while (IsQueueEmpty() && NextChar() != '=' & NextChar() != ';' && !IsSkippable(NextChar())) {
                            typeToken += ConsumeCharacter();
                        }

                        if (KEYWORDS.ContainsKey(typeToken)) {
                            Console.WriteLine("Variable Type: " + KEYWORDS[typeToken]);
                            PushToken(KEYWORDS[typeToken], "VariableDeclarationType");
                            continue;
                        }
                        else RustyErrorHandler.Error($"Unsupported variable type: \"{typeToken}\" on position: (chr: {_pos}, line: {_line})", 5500);
                    }
                    continue;
            }

            if (char.IsDigit(chr)) ReadNumberToken();
            else if (char.IsLetter(chr)) ReadIdentifierToken();
            else if (IsSkippable(chr)) {  continue; }
            else RustyErrorHandler.Error($"Unrecognized character \"{chr}\" on position: (chr: {_pos}, line: {_line})", 550);
        }

        PushToken(TokenType.EOF, "EndOfFile");
        return _tokens;
    }


    #region Chars Queue Actions 
        private bool IsQueueEmpty() => _chars.Count > 0;
        private char NextChar() => _chars.Peek();
        private bool IsSkippable(char z) {
            if(z == ' ' || z == '\n' || z == '\t' || z == '\r') {
                ConsumeCharacter();
                return true;
            }
            return false;
        }
        private char ConsumeCharacter() {
        _pos++;
        return _chars.Dequeue();
    }
    #endregion

    #region Tokens Actions
        private Token Token(TokenType type, string value) => new Token(type, value, _pos, _line);
        private void PushToken(TokenType type, char value) => PushToken(type, value.ToString());
        private void PushToken(TokenType type, string value) => _tokens.Enqueue(Token(type, value));
        private void ReadIdentifierToken() {
            string identifier = string.Empty;

            while (IsQueueEmpty() && char.IsLetter(NextChar())) identifier += ConsumeCharacter();

            if (KEYWORDS.TryGetValue(identifier, out TokenType reservedKeyWord))
                PushToken(reservedKeyWord, identifier);
            else PushToken(TokenType.Identifier, identifier);
        } 
        private void ReadNumberToken() {
            string number = string.Empty;

            while (IsQueueEmpty() && (char.IsDigit(NextChar()) || NextChar() == '.')) number += ConsumeCharacter();

            PushToken(TokenType.Number, number);
        }

    #endregion
}