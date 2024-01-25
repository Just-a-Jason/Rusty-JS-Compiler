using System.Diagnostics;
using System.Reflection;

internal class RSJSCompiler {
    private List<string> imports = new List<string>();
    private IReadOnlyList<Token> _tokens;
    private string _outPath;
    private string? _entry;
        
    public RSJSCompiler(string entry, string outputPath) {
        this._entry = RSJSFileSystem.FindRsJsFile(entry);
        this._outPath = outputPath;
    }

    public void CompileToJavaScript() {
        if(this._entry == null) {
            RSJSErrorHandler.Throw("\tFile not found.", 100);
        }
        else {
            DateTime startTime = DateTime.Now;


            string text = RSJSFileSystem.ReadRsJSFile(this._entry);
            
            Importer importer = new Importer();
            text = importer.ResolveImports(text);

            Tokenizer tokenizer = new Tokenizer();
            this._tokens = tokenizer.TokenizeText(text);

            AbstractSyntaxTree ast = this.GenerateSyntaxTree(); 

            string outJS = this.ProcessTokens(_tokens);

            File.WriteAllText("tokens.txt", "");
            using var fs = new FileStream("tokens.txt", FileMode.OpenOrCreate, FileAccess.Write);
            using var sw = new StreamWriter(fs);

            foreach (var token in this._tokens) sw.WriteLine("{0} {1}", token.text, token.tokenType);

            if (this._outPath == "./" || Path.GetFileName(this._outPath).Trim() == String.Empty) {
                string fileName = Path.GetFileNameWithoutExtension(this._entry); 
                this._outPath += $"{fileName}.js"; 
            }
            else if(Path.GetExtension(this._outPath).Trim() == String.Empty) this._outPath += ".js";
            
            string? path = Directory.GetParent(_outPath).FullName;

            if(!Directory.Exists(path)) Directory.CreateDirectory(path);

            string entrySize = this.GetFileSize(this._entry);

            RSJSFileSystem.SaveFile(this._outPath, outJS);

            string outSize = this.GetFileSize(this._outPath);

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
            Console.WriteLine($"[Compiled in: {compileTime.Milliseconds}ms]");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    public void CompileToRustyJS () { 
    }

    private string ProcessTokens(IReadOnlyList<Token> tokens) {
        string output = String.Empty;

        int i = 0;
        while (i < tokens.Count) {
            output += this.ProcessToken(tokens[i], ref i);
            i++;
        }

        return output;
    }
    private string GetFileSize(string filePath) {
        FileInfo fileInfo = new FileInfo(filePath);
        long fileSizeInBytes = fileInfo.Length;

        return this.FormatFileSize(fileSizeInBytes);
    }

    private string FormatFileSize(long bytes) {
        string[] sizeSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        int i = 0;
        double fileSize = bytes;

        while (fileSize >= 1024 && i < sizeSuffixes.Length - 1)
        {
            fileSize /= 1024;
            i++;
        }

        return $"{fileSize:F2}{sizeSuffixes[i]}";
    }

    private string ProcessToken(Token token, ref int index)
    {
        string output = String.Empty;

        switch (token.tokenType) {
            case TokenType.Keyword:
                if (token.text == "fn") {
                    output += "function ";
                    output += GetNextToken(ref index, 1).text;
                    index++;

                    output += this.ExecuteTokensUntil(ref index, ")") + "){";
                    index++;
                    output += this.ExecuteTokensUntil(ref index, "end");

                    output += "}";
                }

                if (token.text == "class") {
                    output += "class ";
                    output += GetNextToken(ref index, 2).text + "{";
                    index++;

                    output += this.ExecuteTokensUntil(ref index, "end");

                    output += "}";
                }

                if (token.text == "$") return "this";
                if (token.text == "log" && this._tokens[index + 1].text == "!") {
                    output += "console.log";
                    index += 2;

                    output += this.ExecuteTokensUntil(ref index, ")");

                    output += ")";
                }

                if (token.text == "mut") return "var";
                if (token.text == "umut") return "let";
                if (token.text == "init") return "constructor";
                if (token.text == "query") return "querySelector";
                if (token.text == "ret") return "return";
                break;

            case TokenType.Identifier:
            case TokenType.StringLiteral:
            case TokenType.EOL:
            case TokenType.MathOperator:
                return token.text;
        }

        return output;
    }

    private string ExecuteTokensUntil(ref int index, string token) {
        string output = String.Empty;
        while (this._tokens[index].text != token) {
            output += ProcessToken(this._tokens[index], ref index);
            index++;
        }
        return output;
    }
    private Token GetNextToken(ref int index, int skip = 1) {
        index += skip;
        return this._tokens[index];
    } 
    public static string GetCompilerVersion() {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
        return fieVersionInfo.FileVersion;
    }
    private AbstractSyntaxTree GenerateSyntaxTree() {
        RootNode root = new RootNode("Program");

        AbstractSyntaxTree ast = new AbstractSyntaxTree(root);
        


        return ast;
    }
}

