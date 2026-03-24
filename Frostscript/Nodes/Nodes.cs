using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Nodes
{
    internal enum BinaryType { Addition, Subtraction, Multiplication, Devision, Equality, Inequality, GreaterThan, GreaterOrEqual, LessThan, LessOrEqual }
    internal record struct BinaryNode(BinaryType Type, INode Left, INode Right) : INode;
    internal record struct VariableNode(string Label, INode Value, bool Mutable) : INode;
    internal record struct ErrorNode(string Error, Token Token) : INode;
    internal record struct LiteralNode(dynamic Value) : INode;
}
