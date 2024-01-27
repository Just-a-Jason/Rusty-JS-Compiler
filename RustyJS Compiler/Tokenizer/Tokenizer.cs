internal class Tokenizer {

    private Dictionary<string, TokenType> KEYWORDS = new Dictionary<string, TokenType>() {
        { "umut", TokenType.UmutKeyword },
        { "mut", TokenType.MutKeyword },
        { "fn", TokenType.FunctionKeyWord },
        { "end", TokenType.EndKeyWord },
        { "namespace", TokenType.NameSpaceKeyWord },
        { "ret", TokenType.ReturnKeyWord },
        { "rustyCompiler", TokenType.CompilerRuleSet }
    };

    private Queue<char> _chars;
    private long _line = 1;
    private long _pos = 0;

    public Queue<Token> Tokenize(string input) {

        Queue<Token> tokens = new Queue<Token>();
        _chars = new Queue<char>(input.ToCharArray());

        while (_chars.Count > 0) {

            char currentChar = _chars.Peek(); 

            switch (currentChar)
            {
                case '(':
                    tokens.Enqueue(Token(TokenType.OpenPrent, currentChar));
                    Consume();
                    continue;
                case ')':
                    tokens.Enqueue(Token(TokenType.ClosePrent, currentChar));
                    Consume();
                    continue;
                case '*':
                case '+':
                case '-':
                case '/':
                    tokens.Enqueue(Token(TokenType.BinaryOperator, currentChar));
                    Consume();
                    continue;
                case '=':
                    tokens.Enqueue(Token(TokenType.Equals, currentChar));
                    Consume();
                    continue;
            }

            if (char.IsDigit(currentChar))
            {
                string number = string.Empty;

                while (_chars.Count > 0 && (char.IsDigit(_chars.Peek()) || _chars.Peek() == '.'))
                {
                    number += Consume();
                }

                tokens.Enqueue(Token(TokenType.Number, number));
            }

            else if (char.IsLetter(currentChar))
            {
                string identifier = string.Empty;

                while (_chars.Count > 0 && char.IsLetter(_chars.Peek()))
                {
                    identifier += Consume();
                }

                if (KEYWORDS.TryGetValue(identifier, out TokenType reservedKeyWord))
                {
                    tokens.Enqueue(Token(reservedKeyWord, identifier));
                }
                else
                {
                    tokens.Enqueue(Token(TokenType.Identifier, identifier));
                }
            }
            else if (IsSkippable(currentChar))
            {
                Consume();
                continue;
            }
            else
            {
                RustyErrorHandler.Throw($"Unrecognized character \"{currentChar}\" on position: (chr: {_pos}, line: {_line})", 550);
            }
        }

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