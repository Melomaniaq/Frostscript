using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Variable(IExpression Next) : IExpression
    {
        public (INode, Token[]) Parse(INode node, Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Let or TokenType.Var)
            {
                var label = tokens[1];
                var value = Next.Parse(node, [.. tokens.Skip(2)]);
            }
            else Next.Parse(node, tokens);
        }

        public dynamic Interpret(INode node)
        {
            throw new NotImplementedException();
        }
    }
}
