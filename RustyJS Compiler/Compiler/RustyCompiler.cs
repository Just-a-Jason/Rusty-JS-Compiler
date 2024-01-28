using System.Diagnostics;
using System.Reflection;

internal class RustyCompiler {
    private int _compilationTime;
    private string? _entryPath;
    private string _outPath;
        
    public RustyCompiler(string entry, string outputPath) {
        this._entryPath = RustyFileSystem.FindRustyFile(entry);
        this._outPath = outputPath;
    }

    public void CompileToJavaScript() {
        if (this._entryPath == null) RustyErrorHandler.Error("\tFile not found.", 100);
        
        CompilerOptions options = new CompilerOptions();
        DateTime startTime = DateTime.Now;
        
        string content = RustyFileSystem.ReadRustyFile(_entryPath);
            
        RustyImporter importer = new RustyImporter();
        content = importer.ResolveImports(content);

        Tokenizer tokenizer = new Tokenizer();
        Queue<Token> tokens = tokenizer.Tokenize(content);


        RustyParser parser = new RustyParser(tokens);
        parser.ParseTokens();

        this._compilationTime = (DateTime.Now - startTime).Milliseconds;
        SaveOutputFile();
    }

    private void SaveOutputFile() {
        if (this._outPath == "./" || Path.GetFileName(this._outPath).Trim() == String.Empty) {
            string fileName = Path.GetFileNameWithoutExtension(this._entryPath);
            this._outPath += $"{fileName}.js";
        }

        else if (Path.GetExtension(this._outPath).Trim() == String.Empty) this._outPath += ".js";

        string? path = Directory.GetParent(_outPath).FullName;

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        RustyFileSystem.SaveFile(this._outPath, "");

        DisplayOutput(this._compilationTime);
    }

    public void CompileToRusty() { 
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

    public static string GetCompilerVersion() {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
        return fieVersionInfo.FileVersion;
    }
}

