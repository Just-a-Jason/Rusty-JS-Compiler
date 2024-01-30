using Compiler.CompilerSettings;
using System.Text.Json;

class Program {
    #region Rusty Setup
        private const string basicHtmlFile = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n    <head>\r\n        <meta charset=\"UTF-8\">\r\n        <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n        <title>Rusty JS Project</title>\r\n    </head>\r\n    \r\n    <body>\r\n        <script src=\"main.js\"></script>    \r\n    </body>\r\n</html>";
        private const string basicConfigFile = "{\r\n    \"compilerRules\": {\r\n        \"contextIsolation\":true\r\n    },\r\n    \"compilationRules\":{\r\n        \"outputDir\": \"src/build/\",\r\n        \"entry\": \"src/main\"\r\n    }\r\n}";
        private const string htmlFilePath = "src/build/index.html";
        private const string configFile = "rsc.config.json";
    #endregion

    static void Main(string[] args) {
       if(args.Length > 0) RunCLI(args);
       else RunCompilationFromFile();
    }

    static void RunCompilationFromFile() {
        if (!Path.Exists(configFile)) RustyErrorHandler.Error("File \"rsc.config.json\" does not exists! Use: \"RSC --init\" to create it.", 8000);

        RustyRules rules = JsonSerializer.Deserialize<RustyRules>(File.ReadAllText(configFile));

        new RustyCompiler(rules.compilationRules.entry, rules.compilationRules.outputDir).CompileToJavaScript();
    }

    static void RunCLI(string[] args)
    {
        if (args.Length < 1) return;
        string flag = args[0];

        switch (flag)
        {
            case "--init":
                if(!File.Exists("rsc.config.json")) File.WriteAllText(configFile, basicConfigFile);

                if(!Directory.Exists("src/")) {
                    Directory.CreateDirectory("src/");
                    
                    if(!File.Exists("src/main.rsjs")) File.Create("src/main.rsjs");
                    
                    if(!File.Exists(htmlFilePath)) {
                        Directory.CreateDirectory("src/build");
                        File.WriteAllText("src/build/index.html", basicHtmlFile);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("(Rusty JS Compiler) ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Successfully initialized RSC config files.");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "--version":
            case "-v":
                string version = RustyCompiler.GetCompilerVersion();
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write($"» Rusty JS Compiler «");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" (v{version})");
                Console.ResetColor();   
                break;
            case "-f":
                string ouputPath = "./";
                if (args.Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Rusty JS Compiler ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("-f ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("flag requires path to .rsjs file.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                if (args.Length >= 3 && args[2] == "-o")
                {
                    if (args.Length < 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Rusty JS Compiler ");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("-o ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("flag requires path to output .js file.");
                        Console.ForegroundColor = ConsoleColor.White;
                        return;
                    }
                    ouputPath = args[3];
                }
                new RustyCompiler(args[1], ouputPath).CompileToJavaScript();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Flag ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(flag);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" is not a valid flag for RustyJS Compiler.");
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
    }
}