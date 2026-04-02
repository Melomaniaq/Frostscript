
namespace Frostscript.Internal
{
    internal interface ITypedNode
    {
        internal IDataType DataType { get; }
    };

    internal record TypedBinaryNode(BinaryType Type, ITypedNode Left, ITypedNode Right, IDataType DataType) : ITypedNode;
    internal record TypedVariableNode(string Label, ITypedNode Value, bool Mutable, IDataType DataType) : ITypedNode;
    internal record TypedLabelNode(string Label, IDataType DataType) : ITypedNode;
    internal record TypedLiteralNode(dynamic Value, IDataType DataType) : ITypedNode;
    internal record TypedAssignmentNode(string Label, ITypedNode Value, IDataType DataType) : ITypedNode;
    internal record TypedFunctionNode((string Label, IDataType DataType)[] Parameters, ITypedNode Body, IDataType DataType) : ITypedNode;
    internal record TypedCallNode(ITypedNode Left, ITypedNode Right, IDataType DataType) : ITypedNode;
    internal record TypedParenthesesNode(ITypedNode Body, IDataType DataType) : ITypedNode;
}
