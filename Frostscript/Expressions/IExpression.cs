using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Frostscript.Expressions
{
    internal interface IExpression
    {
        public (INode, Token[]) Parse(Token[] tokens);
        public dynamic Interpret(INode node, IDictionary<string, INode> variables);
    }
}
