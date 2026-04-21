namespace Frostscript.Domain.Validator
{
    public record ValidationError(Token Token, string Error);
}
