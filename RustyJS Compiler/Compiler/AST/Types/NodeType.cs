internal enum NodeType {
    // Statements
    Program, 
    VariableDeclaration,
    
    //Literals
    RustyProperty,
    NumericLiteral, 
    ObjectLiteral,
    NullLiteral,

    // Expressions
    AssignmentExpression,
    BinaryExpression,
    Identifier, 
}

