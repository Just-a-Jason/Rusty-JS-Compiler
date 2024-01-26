internal class RustyImporter {
    private Dictionary<string, string> _imports = new Dictionary<string, string>();

    private const string IMPORT_KEYWORD = "@import";
    
    private Tokenizer _tokenizer = new Tokenizer();
    
    private string _outFile = String.Empty;
    

    private void ResolveImport(string import, List<string> outFile) {
        if (_imports.Keys.Contains(import)) { outFile.Remove(import); return; }

        Queue<Token> tokens = this._tokenizer.TokenizeText(import);
        string importPath = String.Empty;
        string? path;

        Token token = tokens.Dequeue();
        while (token.Text != ";") { 
            token = tokens.Dequeue();
            importPath += token.Text;
        };

        importPath = importPath.Trim();
        path = RustyFileSystem.FindRustyFile(importPath);

        if (path == null) RustyErrorHandler.Throw($"File: {Path.GetFileName(importPath)} does not exists.", 200);

        
        _imports.Add(import,importPath);
        outFile.InsertRange(0,RustyFileSystem.ReadRustyFile(path).Split("\n").ToList()); 
    }
    private string JoinContent(List<string> lines) {
        string output = string.Empty;
        foreach(string line in lines) output += line + "\n";
        return output;
    }
    public string ResolveImports(string content) {
        
        this._outFile = content;
        while(_outFile.Contains(IMPORT_KEYWORD)) this._outFile = this.ImportFile();

        return this._outFile;
    }
    private string ImportFile() {
        List<string> lines = _outFile.Split("\n").ToList();
        List<string> outFile = new List<string>(lines);

        foreach (string line in lines) { 
            if (!line.Contains(IMPORT_KEYWORD)) continue;
            ResolveImport(line, outFile);
        }
       
        return this.JoinContent(outFile);
    }
}