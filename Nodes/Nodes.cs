using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Nodes
{
    internal record struct ErrorNode(string Error) : INode;
    internal record struct LiteralNode(dynamic Value) : INode;
    internal record struct StatementNode : INode;
}
