
namespace Frostscript.Internal
{
    internal interface INode 
    { 
        internal IDataType DataType { get;  } 
        internal Token Token { get;  } 
    };
    internal enum BinaryType 
    { 
        Addition, 
        Subtraction, 
        Multiplication, 
        Devision, 
        Equality, 
        Inequality, 
        GreaterThan, 
        GreaterOrEqual, 
        LessThan, 
        LessOrEqual,
        And,
        Or
    }

    internal record BinaryNode(BinaryType Type, INode Left, INode Right, IDataType DataType, Token Token) : INode;
    internal record VariableNode(string Label, INode Value, bool Mutable, IDataType DataType, Token Token) : INode;
    internal record ErrorNode(string Error, IDataType DataType, Token Token) : INode;
    internal record LabelNode(string Label, IDataType DataType, Token Token) : INode;
    internal record LiteralNode(dynamic Value, IDataType DataType, Token Token) : INode;
    internal record AssignmentNode(string Label, INode Value, IDataType DataType, Token Token) : INode;
    internal record FunctionNode(string[] Parameters, INode Body, IDataType DataType, Token Token) : INode;
    internal record CallNode(INode Left, INode Right, IDataType DataType, Token Token) : INode;
    internal record ParenthesesNode(INode Body, IDataType DataType, Token Token) : INode;
}
