using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Function(IExpression Next) : IExpression
    {
        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            throw new NotImplementedException();
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is not TokenType.Fun)
                return Next.Parse(tokens);

            if (tokens[1].Type is not TokenType.Label )
                return (new ErrorNode("Expected Parameter",  tokens[1]), []);

            var parameters =
                tokens
                .Skip(1)
                .TakeWhile(x => x.Type == TokenType.Label)
                .Select(x => x.Literal as string ?? throw new NullReferenceException())
                .ToArray();

            var newTokens = tokens.Skip(parameters.Length + 1).ToArray();

            if (newTokens[0].Type is not TokenType.Arrow)
                return (new ErrorNode("Expected '->' ", newTokens[0]), []);

            var (body, bodyTokens) = Parse([.. newTokens.Skip(1)]);
            return (new FunctionNode(parameters, body), bodyTokens);
        }
    }
}
