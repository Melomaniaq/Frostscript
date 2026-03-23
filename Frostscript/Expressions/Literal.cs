using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Literal : IExpression
    {
        public dynamic Interpret(INode node)
        {
            if (node is LiteralNode literal) return literal.Value;
            else throw new NotImplementedException();
        }

        public (INode, Token[]) Parse(INode node, Token[] tokens)
        {
            if (tokens[0].Type != TokenType.Literal)
                return (new ErrorNode($"[{tokens[0].Line}:{tokens[0].Character}]Unexpected token {tokens[0].Literal}"), tokens);

            return (new LiteralNode(tokens[0].Literal), [.. tokens.Skip(1)]);
        }
    }
}
