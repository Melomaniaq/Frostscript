using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Error : IExpression
    {
        public dynamic Interpret(INode node)
        {
            throw new NotImplementedException();
        }

        public (INode, Token[]) Parse(INode node, Token[] tokens) => 
            (new ErrorNode($"[{tokens[0].Line}:{tokens[0].Character}]Unexpected token {tokens[0].Literal}"), tokens);
    }
}
