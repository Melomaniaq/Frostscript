using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal interface IExpression
    {
        public (INode, Token[]) Parse(INode node, Token[] tokens);
        public dynamic Interpret(INode node);
    }
}
