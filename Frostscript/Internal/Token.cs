namespace Frostscript.Internal
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
        LessThan,
        And,
        Or,
        Fun,
        Arrow,
        SemiColon,
        Colon,
        ParenthesesOpen,
        ParenthesesClose,
        Num,
        Bool,
        Str
    }

    internal record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
