
namespace Frostscript.Domain.Internal
{
    public interface INode 
    { 
        public Token Token { get;  } 
    };
  
    public record BinaryNode(BinaryType Type, INode Left, INode Right, Token Token) : INode;
    public record VariableNode(string Label, INode Value, bool Mutable, Token Token) : INode;
    public record ErrorNode(string Error, Token Token) : INode;
    public record LabelNode(string Label, Token Token) : INode;
    public record LiteralNode(dynamic Value, Token Token) : INode;
    public record AssignmentNode(string Label, INode Value, Token Token) : INode;
    public record FunctionNode((string label, IDataType dataType)[] Parameters, INode Body, Token Token) : INode;
    public record CallNode(INode Left, INode Right, Token Token) : INode;
    public record ParenthesesNode(INode Body, Token Token) : INode;
}
