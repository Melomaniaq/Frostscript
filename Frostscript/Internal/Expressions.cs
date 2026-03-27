
namespace Frostscript.Internal
{
    internal interface IExpression
    {
        internal IDataType DataType { get; }
    };

    internal record BinaryExpression(BinaryType Type, IExpression Left, IExpression Right, IDataType DataType) : IExpression;
    internal record VariableExpression(string Label, IExpression Value, bool Mutable, IDataType DataType) : IExpression;
    internal record ErrorExpression(string Error, IDataType DataType) : IExpression;
    internal record LabelExpression(string Label, IDataType DataType) : IExpression;
    internal record LiteralExpression(dynamic Value, IDataType DataType) : IExpression;
    internal record AssignmentExpression(string Label, IExpression Value, IDataType DataType) : IExpression;
    internal record FunctionExpression(string[] Parameters, IExpression Body, IDataType DataType) : IExpression;
    internal record CallExpression(IExpression Left, IExpression Right, IDataType DataType) : IExpression;
    internal record ParenthesesExpression(IExpression Body, IDataType DataType) : IExpression;
}
