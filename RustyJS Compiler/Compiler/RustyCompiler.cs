using System.Diagnostics;
using System.Reflection;


internal class RustyCompiler {
    private List<string> imports = new List<string>();
    private IReadOnlyList<Token> _tokens;
    private int _currentToken = 0;
    private string _outPath;
    private string? _entry;
        
    public RustyCompiler(string entry, string outputPath) {
        this._entry = RustyFileSystem.FindRsJsFile(entry);
        this._outPath = outputPath;
    }

    public void CompileToJavaScript() {
        if (this._entry == null) RustyErrorHandler.Throw("\tFile not found.", 100);
        DateTime startTime = DateTime.Now;
        
        string text = RustyFileSystem.ReadRustyFile(this._entry);
            
        Importer importer = new Importer();
        text = importer.ResolveImports(text);

        Tokenizer tokenizer = new Tokenizer();
        this._tokens = tokenizer.TokenizeText(text);

        this.SaveTokens();

        // Context isolation

        string outJS = this.ProcessTokens(_tokens);
        if (outJS.Trim() != String.Empty) outJS = "(()=>{" + outJS + "})();";

        if (this._outPath == "./" || Path.GetFileName(this._outPath).Trim() == String.Empty) {
            string fileName = Path.GetFileNameWithoutExtension(this._entry); 
            this._outPath += $"{fileName}.js"; 
        }

        else if(Path.GetExtension(this._outPath).Trim() == String.Empty) this._outPath += ".js";
            
        string? path = Directory.GetParent(_outPath).FullName;

        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        string entrySize = RustyFileSystem.GetFileSize(this._entry);

        RustyFileSystem.SaveFile(this._outPath, outJS);

        string outSize = RustyFileSystem.GetFileSize(this._outPath);

        TimeSpan compileTime = DateTime.Now - startTime;

        Console.ForegroundColor = ConsoleColor.Blue;
        string inputPathFileName = Path.GetFileName(this._entry);
        Console.Write($"[{inputPathFileName}]");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"({entrySize})");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($" => ");

        Console.ForegroundColor = ConsoleColor.Yellow;
        string outPathFileName = Path.GetFileName(this._outPath);
        Console.Write($"[{outPathFileName}]");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"({outSize})");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"[Compiled in: ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{compileTime.Milliseconds}ms");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("]");
    }

    public void CompileToRustyJS () { 
    }

    private void SaveTokens() {
        File.WriteAllText("tokens.txt", "");
        using var fs = new FileStream("tokens.txt", FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(fs);

        foreach (var token in this._tokens) sw.WriteLine("{0} {1}", token.Text, token.TokenType);
    }

    private string ProcessTokens(IReadOnlyList<Token> tokens) {
        string output = String.Empty;

        while (this._currentToken < tokens.Count) {
            output += this.ProcessToken(tokens[this._currentToken]);
            this._currentToken++;
        }

        return output;
    }
  

    private string ProcessToken(Token token)
    {
        string output = String.Empty;

        switch (token.TokenType) {
            case TokenType.Keyword:
                if (token.Text == "fn") {
                    output += "function ";
                    output += GetNextToken().Text;
                    this._currentToken++;

                    output += this.ExecuteTokensUntil( ")") + "){";
                    this._currentToken++;
                    output += this.ExecuteTokensUntil( "end");

                    output += "}";
                }

                if (token.Text == "class") {
                    output += "class ";
                    output += GetNextToken(1).Text + "{";
                    this._currentToken++;

                    output += this.ExecuteTokensUntil("end");

                    output += "}";
                }

                if (token.Text == "$") return "this";
                if (token.Text == "log" && this._tokens[this._currentToken + 1].Text == "!") {
                    output += "console.log";
                    this._currentToken += 2;

                    output += this.ExecuteTokensUntil(")");

                    output += ")";
                }

                if (token.Text == "mut") return "var ";
                if (token.Text == "umut") return "let ";
                if (token.Text == "init") return "constructor";
                if (token.Text == "query") return "querySelector";
                if (token.Text == "ret") return "return ";
                if (token.Text == "new") return "new ";
                if (token.Text == "static") return "static ";
                break;

            case TokenType.MathOperator:
            case TokenType.StringLiteral:
            case TokenType.Identifier:
            case TokenType.Number:
            case TokenType.EOL:
                return token.Text;
        }

        return output;
    }

    private string ExecuteTokensUntil(string token) {
        string output = String.Empty;
        while (this._tokens[this._currentToken].Text != token) {
            output += ProcessToken(this._tokens[this._currentToken]);
            this._currentToken++;
        }
        return output;
    }
    private Token GetNextToken(int skip = 1) {
        this._currentToken += skip;
        return this._tokens[this._currentToken];
    } 
    public static string GetCompilerVersion() {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
        return fieVersionInfo.FileVersion;
    }   
}

