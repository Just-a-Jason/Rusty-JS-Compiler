namespace Compiler.CompilerSettings.CompilerRulesTypes {
    internal class CompilationRules {
        public string? outputDir { get; set; } = "src/build";
        public List<string>? includePaths { get; set; } = null;
        public string entry { get; set; } = "src/main";
    }
}
