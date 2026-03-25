using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Literal : IExpression
    {
        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type != TokenType.Literal)
                return (new ErrorNode($"Unexpected token {tokens[0].Literal}", tokens[0]), tokens);

            return (new LiteralNode(tokens[0].Literal), [.. tokens.Skip(1)]);
        }
        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            if (node is LiteralNode literal) return literal.Value;
            else throw new NotImplementedException();
        }
    }
}
