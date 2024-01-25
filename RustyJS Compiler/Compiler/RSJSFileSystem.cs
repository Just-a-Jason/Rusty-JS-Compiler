internal static class RSJSFileSystem {
    public static string? FindRsJsFile(string path) {
        if (Path.Exists(path)) return path;

        string fileName = Path.GetFileNameWithoutExtension(path);
        string ext = Path.GetExtension(path);

        if (ext.Trim() == String.Empty) fileName += ".rsjs";

        foreach (string file in Directory.GetFiles(Directory.GetParent(path).FullName))
            if(Path.GetFileName(file) == fileName) return file;
        
        return null;
    }

    public static string ReadRsJSFile(string path) {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        using var sr = new StreamReader(fs);

        return sr.ReadToEnd();
    }

    public static void SaveFile(string path, string content) {
        File.WriteAllText(path, String.Empty);
        using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        using var sw = new StreamWriter(fs);

        sw.Write(content);
    }
}