namespace Frostscript
{
    internal enum TokenType { Literal, Label, Plus, Minus, ForwardSlash, Star, SingleEqual, DoubleEqual, NotEqual, Not }
    internal record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
