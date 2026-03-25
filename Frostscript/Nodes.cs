using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript
{
    public interface INode { }
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
    internal record struct BinaryNode(BinaryType Type, INode Left, INode Right) : INode;
    internal record struct VariableNode(string Label, INode Value, bool Mutable) : INode;
    internal record struct ErrorNode(string Error, Token Token) : INode;
    internal record struct LabelNode(string Label) : INode;
    internal record struct LiteralNode(dynamic Value) : INode;
    internal record struct AssignmentNode(string Label, INode Value) : INode;
    internal record struct FunctionNode(string[] Parameters, INode Body) : INode;
    internal record struct CallNode(INode Left, INode Right) : INode;
    internal record struct ParenthesesNode(INode Body) : INode;
}
