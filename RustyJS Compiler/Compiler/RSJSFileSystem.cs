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

    public static string GetFileSize(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        long fileSizeInBytes = fileInfo.Length;

        return FormatFileSize(fileSizeInBytes);
    }

    private static string FormatFileSize(long bytes) {
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
}