internal class RustyErrorHandler {
    public static void Error(string error, int errorNo) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("╔══════════════════════════════════════════════╗\n");
        Console.Write("║");
        Console.Write("   (Rusty JS Compiler)   ");
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write($" RSJS:{errorNo.ToString("X")} \n");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("║");
        Console.Write($"   {error}   ");
        Console.Write("║\n");
        Console.Write("╚══════════════════════════════════════════════╝\n");
        Console.ForegroundColor = ConsoleColor.White;

        Environment.Exit(1);
    }

    public static void Warning() {

    }
}
