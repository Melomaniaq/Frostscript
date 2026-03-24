namespace Frostscript
{
    internal enum TokenType { 
        Literal, 
        Label,
        Id,
        Let,
        Var,
        Plus, 
        Minus, 
        ForwardSlash, 
        Star, 
        SingleEqual, 
        DoubleEqual, 
        NotEqual, 
        Not,
        GreaterThan,
        GreaterOrEqual,
        LessOrEqual,
        LessThan
    }

    internal record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
