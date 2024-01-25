internal class RSJSErrorHandler {
    public static void Throw(string error, int errorNo) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("(Rusty JS Compiler)");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($" RSJS:{errorNo} \n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(error);
        Console.ForegroundColor = ConsoleColor.White;
        Environment.Exit(1);
    }
}
