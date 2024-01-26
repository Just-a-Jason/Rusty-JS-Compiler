using System.Diagnostics;
using System.Reflection;


internal class RustyCompiler {
    private List<string> imports = new List<string>();
    private Queue<Token> _tokens;
    private int _currentToken = 0;
    private string _outPath;
    private string? _entryPath;

    private RustyParser parser;
        
    public RustyCompiler(string entry, string outputPath) {
        this._entryPath = RustyFileSystem.FindRustyFile(entry);
        this._outPath = outputPath;
    }

    public void CompileToJavaScript() {
        if (this._entryPath == null) RustyErrorHandler.Throw("\tFile not found.", 100);
        DateTime startTime = DateTime.Now;
        
        string text = RustyFileSystem.ReadRustyFile(this._entryPath);
            
        RustyImporter importer = new RustyImporter();
        text = importer.ResolveImports(text);

        Tokenizer tokenizer = new Tokenizer();
        this._tokens = tokenizer.TokenizeText(text);

        this.SaveTokens();

        // Context isolation
        string outJS = this.ProcessTokens();

        if (RustyFileSystem.IsFileEmpty(outJS)) outJS = "(()=>{" + outJS + "})();";

        if (this._outPath == "./" || Path.GetFileName(this._outPath).Trim() == String.Empty) {
            string fileName = Path.GetFileNameWithoutExtension(this._entryPath); 
            this._outPath += $"{fileName}.js"; 
        }

        else if(Path.GetExtension(this._outPath).Trim() == String.Empty) this._outPath += ".js";
            
        string? path = Directory.GetParent(_outPath).FullName;

        if(!Directory.Exists(path)) Directory.CreateDirectory(path);

        RustyFileSystem.SaveFile(this._outPath, outJS);
        TimeSpan compileTime = DateTime.Now - startTime;

        DisplayOutput(compileTime.Milliseconds);
    }

    public void CompileToRusty () { 
    }

    private void DisplayOutput(int milisecounds) {
        Console.Write($"[");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{this._entryPath}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" ({RustyFileSystem.GetFileSize(this._entryPath)}) ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("=>");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" [{this._outPath}]");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" ({RustyFileSystem.GetFileSize(this._outPath)}) ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[Compiled in: ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{milisecounds}ms");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("]\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void SaveTokens() {
        File.WriteAllText("tokens.txt", "");
        using var fs = new FileStream("tokens.txt", FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(fs);

        foreach (var token in this._tokens) sw.WriteLine("{0} {1}", token.Text, token.TokenType);
    }

    private string ProcessTokens() {
        string output = String.Empty;

        while (_tokens.Count > 0) output += ProcessToken(_tokens.Dequeue());

        return output;
    }
  

    private string ProcessToken(Token token) {
        string output = String.Empty;

        switch (token.TokenType) {
            case TokenType.Keyword:
                if (token.Text == "fn") {
                    output += "function ";
                    output += ConsumeToken().Text;
                    this._currentToken++;

                    output += ExecuteTokensUntil( ")") + "){";
                    this._currentToken++;
                    output += ExecuteTokensUntil( "end");

                    output += "}";
                }

                if (token.Text == "class") {
                    output += "class ";
                    output += ConsumeToken().Text + "{";

                    output += ExecuteTokensUntil("end");

                    output += "}";
                }

                if (token.Text == "$") return "this";
                if (token.Text == "log" && ConsumeToken().Text == "!") {
                    output += "console.log";

                    output += ExecuteTokensUntil(")");

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

        while (ConsumeToken().Text != token) output += ProcessToken(ConsumeToken());
        
        return output;
    }

    public static string GetCompilerVersion() {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
        return fieVersionInfo.FileVersion;
    }

    private Token ConsumeToken() => this._tokens.Dequeue();
}

