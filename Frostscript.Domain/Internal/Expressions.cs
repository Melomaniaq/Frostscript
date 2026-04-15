
namespace Frostscript.Domain.Internal
{
    public interface IExpression;

    public record BinaryExpression(BinaryType Type, IExpression Left, IExpression Right) : IExpression;
    public record VariableExpression(string Label, IExpression Value) : IExpression;
    public record LabelExpression(string Label) : IExpression;
    public record LiteralExpression(dynamic Value) : IExpression;
    public record AssignmentExpression(string Label, IExpression Value) : IExpression;
    public record FunctionExpression(string[] Parameters, IExpression Body) : IExpression;
    public record CallExpression(IExpression Left, IExpression Right) : IExpression;
    public record ParenthesesExpression(IExpression Body) : IExpression;
}
