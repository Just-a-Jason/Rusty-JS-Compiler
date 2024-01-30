using Compiler.CompilerSettings;
using System.Diagnostics;
using System.Reflection;

internal class RustyCompiler {
    private RustyRules? _rules;
    private int _compilationTime;
    private string? _entryPath;
    private string _outPath;
        
    public RustyCompiler(string entry, string outputPath) {
        _entryPath = RustyFileSystem.FindRustyFile(entry);
        _outPath = outputPath;
    }

    public RustyCompiler(RustyRules? rules) {
        _rules = rules;
        if (rules == null) RustyErrorHandler.Error("\tInvalid syntax in rsc.json.config cannot load config.", 800);

        if (rules.compilationRules.entry == null) RustyErrorHandler.Error("\tFile not found.", 100);
        else _entryPath = RustyFileSystem.FindRustyFile(rules.compilationRules.entry);

        _outPath = (rules.compilationRules.outputDir == null) ? "./" : rules.compilationRules.outputDir;

    }

    public void CompileToJavaScript() {
        if (_entryPath == null) RustyErrorHandler.Error("\tFile not found.", 100);
        
        DateTime startTime = DateTime.Now;
        
        string content = RustyFileSystem.ReadRustyFile(_entryPath);
            
        //RustyImporter importer = new RustyImporter();
        //content = importer.ResolveImports(content);

        RustyTokenizer tokenizer = new RustyTokenizer();
        Queue<Token> tokens = tokenizer.Tokenize(content);


        RustyParser parser = new RustyParser(tokens);
        string outJs = parser.ParseTokens();
        if (_rules != null && _rules.compilerRules.contextIsolation) outJs = $"(()=>{{{outJs}}})();";

        _compilationTime = (DateTime.Now - startTime).Milliseconds;
        SaveOutputFile(outJs);
    }

    private void SaveOutputFile(string outJS) {
        if (_outPath == "./" || Path.GetFileName(_outPath).Trim() == String.Empty) {
            string fileName = Path.GetFileNameWithoutExtension(this._entryPath);
            _outPath += $"{fileName}.js";
        }

        else if (Path.GetExtension(this._outPath).Trim() == String.Empty) this._outPath += ".js";

        string? path = Directory.GetParent(_outPath).FullName;

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        RustyFileSystem.SaveFile(_outPath, outJS);

        DisplayOutput(_compilationTime);
    }

    public void CompileToRusty() { 
    }

    private void DisplayOutput(int milisecounds) {
        Console.Write($"[");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{_entryPath}");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("]");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" ({RustyFileSystem.GetFileSize(_entryPath)}) ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("=>");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" [{_outPath}]");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($" ({RustyFileSystem.GetFileSize(_outPath)}) ");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("[Compiled in: ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{milisecounds}ms");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("]\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static string GetCompilerVersion() {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
        return fieVersionInfo.FileVersion;
    }
}

