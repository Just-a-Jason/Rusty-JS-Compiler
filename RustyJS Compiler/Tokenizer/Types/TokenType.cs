internal enum TokenType {
    // ValueType Tookens
    Identifier,
    Number,
    Colon,

    // Operators
    BinaryOperator, 
    ClosePrent, 
    OpenPrent, 
    Equals,
    
    // Variable Declarations
    ConstantKeyWord,
    UmutKeyword, 
    MutKeyword, 
    
    // Scopes
    NameSpaceKeyWord,
    EndKeyWord,

    // Functions 
    FunctionKeyWord, 
    ReturnKeyWord, 
    

    // Constant Values
    BooleanValue, 
    
    // VariableTypes
    Null, 
    Int8, Int16, 
    Int32, Int64, 
    UInt8, UInt16, 
    UInt32, UInt64,
    Float32, Float64, 
    String, 
    Char, Bool, 
    Object, Auto, 
    Array,

    // Terimnators
    Semi,
    EOF
}

