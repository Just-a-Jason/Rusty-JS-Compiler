internal class Importer {
    private Dictionary<string, string> _imports = new Dictionary<string, string>();
    private Tokenizer _tokenizer = new Tokenizer();
    private const string IMPORT_KEYWORD = "@import";
    private string _outFile = String.Empty;

    public string ResolveImports(string content) {
        
        this._outFile = content;
        while(_outFile.Contains(IMPORT_KEYWORD)) this._outFile = this.ImportFile();


        return this._outFile;
    }
    private string ImportFile() {
        List<string> lines = this._outFile.Split("\n").ToList();
        List<string> outFile = new List<string>(lines);

        foreach (string line in lines) { 
            if (!line.Contains("@import")) continue;
            ResolveImport(line, outFile);
        }
       
        return this.JoinContent(outFile);
    }

    private void ResolveImport(string import, List<string> outFile) {
        if (this._imports.Keys.Contains(import)) { outFile.Remove(import); return; }

        IReadOnlyList<Token> tokens = this._tokenizer.TokenizeText(import);
        string importPath = String.Empty;
        string? path;
        int i = 1;

        while (tokens[i].text != ";") importPath += tokens[i++].text;

        importPath = importPath.Trim();
        path = RSJSFileSystem.FindRsJsFile(importPath);

        if (path == null) RSJSErrorHandler.Throw($"File: {Path.GetFileName(importPath)} does not exists.", 200);

        
        this._imports.Add(import,importPath);
        outFile.InsertRange(0,RSJSFileSystem.ReadRsJSFile(path).Split("\n").ToList()); 
    }
    private string JoinContent(List<string> lines) {
        string output = string.Empty;
        foreach(string line in lines) output += line + "\n";
        return output;
    }
}
