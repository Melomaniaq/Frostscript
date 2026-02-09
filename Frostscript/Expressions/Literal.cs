using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Literal(IExpression next) : IExpression
    {
        public dynamic Interpret(INode node)
        {
            if (node is LiteralNode literal) return literal.Value;
            else return next.Interpret(node);
        }

        public (INode, Token[]) Parse(INode node, Token[] tokens)
        {
            if (tokens[0].Type != TokenType.Literal)
                return next.Parse(node, tokens);

            return (new LiteralNode(tokens[0].Literal), [.. tokens.Skip(1)]);
        }
    }
}
