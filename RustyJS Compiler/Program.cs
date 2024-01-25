using System.Diagnostics;
using System.Reflection;

class Program {
    static void Main(string[] args) {
       RunCLI(args);
    }
    static void RunCLI(string[] args) {
        if (args.Length < 1) return;
        string flag = args[0];

        switch (flag) {
            case "--version":
            case "-v":
                string version = RSJSCompiler.GetCompilerVersion();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Rusty JS Compiler");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" (v{version})");
                Console.ForegroundColor = ConsoleColor.White;
                break;
            case "-f":
                string ouputPath = "./";
                if (args.Length < 2) {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Rusty JS Compiler ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("-f ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("flag requires path to .rsjs file.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }

                if (args.Length >= 3 && args[2] == "-o") {
                    if (args.Length < 4) {
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
                new RSJSCompiler(args[1], ouputPath).CompileToJavaScript();
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