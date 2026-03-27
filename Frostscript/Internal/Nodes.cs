
namespace Frostscript.Internal
{
    internal interface INode 
    { 
        internal Token Token { get;  } 
    };
  
    internal record BinaryNode(BinaryType Type, INode Left, INode Right, Token Token) : INode;
    internal record VariableNode(string Label, INode Value, bool Mutable, Token Token) : INode;
    internal record ErrorNode(string Error, Token Token) : INode;
    internal record LabelNode(string Label, Token Token) : INode;
    internal record LiteralNode(dynamic Value, Token Token) : INode;
    internal record AssignmentNode(string Label, INode Value, Token Token) : INode;
    internal record FunctionNode(string[] Parameters, INode Body, Token Token) : INode;
    internal record CallNode(INode Left, INode Right, Token Token) : INode;
    internal record ParenthesesNode(INode Body, Token Token) : INode;
}
