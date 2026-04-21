namespace Frostscript.Domain
{
    public enum TokenType { 
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

    public record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
