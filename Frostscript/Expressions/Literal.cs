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
                return (new ErrorNode($"Unexpected token {tokens[0].Literal}", tokens[0]), [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

            return (new LiteralNode(tokens[0].Literal), [.. tokens.Skip(1)]);
        }
        public dynamic Interpret(INode node, IDictionary<string, INode> variables)
        {
            if (node is LiteralNode literal) return literal.Value;
            if (node is ErrorNode error) throw new Exception($"Unhandled Parsing Error: {error.Error}");
            else throw new NotImplementedException("Node Could not be resolved. Did you forget to add the expression to the expression tree?");
        }
    }
}
