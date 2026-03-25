using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Function : IExpression
    {
        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            throw new NotImplementedException();
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            throw new NotImplementedException();
        }
    }
}
