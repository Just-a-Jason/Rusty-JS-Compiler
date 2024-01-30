using Compiler.CompilerSettings.CompilerRulesTypes;

namespace Compiler.CompilerSettings {
    internal class RustyRules {
        public CompilationRules compilationRules { get; set; } = new CompilationRules();
        public CompilerRules compilerRules { get; set; } = new CompilerRules();
    }
}

