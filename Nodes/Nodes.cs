using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Nodes
{
    internal enum BinaryType { Addition, Subtraction, Multiplication, Devision }
    internal record struct BinaryNode(BinaryType Type, INode Left, INode Right) : INode;
    internal record struct ErrorNode(string Error) : INode;
    internal record struct LiteralNode(dynamic Value) : INode;

    internal record struct StatementNode : INode;
}
