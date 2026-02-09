namespace Frostscript
{
    enum TokenType { Literal, Label, Plus, Minus, Cross, ForwardSlash, Star }
    internal record struct Token(TokenType Type, int Line, int Character, dynamic? Literal = null);
}
