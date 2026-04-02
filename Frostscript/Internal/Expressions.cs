
namespace Frostscript.Internal
{
    internal interface IExpression;

    internal record BinaryExpression(BinaryType Type, IExpression Left, IExpression Right) : IExpression;
    internal record VariableExpression(string Label, IExpression Value, bool Mutable) : IExpression;
    internal record LabelExpression(string Label) : IExpression;
    internal record LiteralExpression(dynamic Value) : IExpression;
    internal record AssignmentExpression(string Label, IExpression Value) : IExpression;
    internal record FunctionExpression(string[] Parameters, IExpression Body) : IExpression;
    internal record CallExpression(IExpression Left, IExpression Right) : IExpression;
    internal record ParenthesesExpression(IExpression Body) : IExpression;
}
