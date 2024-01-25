using System.Runtime.CompilerServices;

internal class Tokenizer {
    private const string SEPARATORS = "(){}&<>!=+/.$,;\'\" ";

    private static readonly string[] keywords = new string[] {
       "fn", "ret", 
       "end", "base", 
       "$", "namespace",
       "class", "log", "mut", 
       "init", "query", "umut"
    };


    public IReadOnlyList<Token> TokenizeText(string text) {
        string [] lines = text.Split('\n');
        List<Token> tokens = new List<Token>();

        foreach (string line in lines) {
            if (line.Trim() == String.Empty || line.StartsWith('#')) continue;
            tokens.AddRange(Tokenize(line));
        }

        return tokens.AsReadOnly();
    }

    private List<Token> Tokenize(string line)
    {
        List<Token> tokens = new List<Token>();

        string token = string.Empty;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '\t') continue;

            if (SEPARATORS.Contains(c))
            {
                token = token.Trim();
                if (!string.IsNullOrEmpty(token))
                {
                    tokens.Add(new Token(GetTokenType(token), token));
                }

                token = string.Empty;
                tokens.Add(new Token(GetTokenType(c.ToString()), c.ToString()));
            }
            else
            {
                token += c;
            }
        }

        token = token.Trim();
        if (!string.IsNullOrEmpty(token))
        {
            tokens.Add(new Token(GetTokenType(token), token));
        }

        return tokens;
    }

    private TokenType GetTokenType(string token) {
        if (keywords.Contains(token)) return TokenType.Keyword;
        if (token.StartsWith("@")) return TokenType.Decorator;

        switch (token) {
            case "+": case "-":
            case "*": case "/":
                return TokenType.MathOperator;
            case "0": case "1":
            case "2": case "3":
            case "4": case "5":
            case "6": case "7":
            case "8": case "9":
                return TokenType.Number;
            case "'":
            case "\"":
                return TokenType.StringLiteral;
            case "str": case "i32": case "f32":
                return TokenType.VariableType;
            case "public": case "private": case "protected":
                return TokenType.AccessModifier;
            case "end": return TokenType.EndToken;
            case ";": return TokenType.EOL;
            default: return TokenType.Identifier;
        }
    }
}