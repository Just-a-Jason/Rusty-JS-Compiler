internal class RustyParser {
    private Queue<Token> _tokens;
    
    public RustyParser(Queue<Token> tokens) {
        _tokens = tokens;    
    }

    public void ParseTokens(){

    }

    private AbstractSyntaxTree GenerateAST() {
        AbstractSyntaxTree tree = new AbstractSyntaxTree();
        return tree;
    }


}

