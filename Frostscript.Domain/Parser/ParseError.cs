namespace Frostscript.Domain.Parser
{
    public class ParseError(Token token, string message, Token[] currentToken)
    {
        public Token Token { get; } = token;
        public string Message { get; } = message;
        public Token[] RemainingTokens { get; } = [.. currentToken.SkipWhile(x => x.Type is not TokenType.SemiColon).Skip(1)];
    }
}