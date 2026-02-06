namespace Frostscript
{
    enum TokenType { Literal, Label }
    internal record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
