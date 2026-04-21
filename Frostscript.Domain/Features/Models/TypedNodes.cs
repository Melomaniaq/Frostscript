using Frostscript.Domain.Parameters;

namespace Frostscript.Domain.Features.Models
{
    public interface ITypedNode
    {
        public IDataType DataType { get; }
    };

    public record TypedBinaryNode(BinaryType Type, ITypedNode Left, ITypedNode Right, IDataType DataType) : ITypedNode;
    public record TypedVariableNode(string Label, ITypedNode Value, bool Mutable, IDataType DataType) : ITypedNode;
    public record TypedLabelNode(string Label, IDataType DataType) : ITypedNode;
    public record TypedLiteralNode(dynamic Value, IDataType DataType) : ITypedNode;
    public record TypedAssignmentNode(string Label, ITypedNode Value, IDataType DataType) : ITypedNode;
    public record TypedFunctionNode(Parameter[] Parameters, ITypedNode Body, IDataType DataType) : ITypedNode;
    public record TypedCallNode(ITypedNode Left, ITypedNode Right, IDataType DataType) : ITypedNode;
    public record TypedParenthesesNode(ITypedNode Body, IDataType DataType) : ITypedNode;
}
