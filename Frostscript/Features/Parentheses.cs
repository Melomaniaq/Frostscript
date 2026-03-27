using Frostscript.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Features
{
    internal class Parentheses(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is ParenthesesNode parentheses)
                return ExpressionTree.ExpressionTree.Interpret(parentheses.Body, variables);

            return Next.Interpret(expression, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is not TokenType.ParenthesesOpen)
                return Next.Parse(tokens);

            var (body, bodyTokens) = ExpressionTree.ExpressionTree.Parse([.. tokens.Skip(1)]);

            if (bodyTokens.Length != 0 && bodyTokens[0].Type is not TokenType.ParenthesesClose)
                return (new ErrorNode("Expected ')'", bodyTokens[0]), [.. bodyTokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

            return (new ParenthesesNode(body, tokens[0]), [.. bodyTokens.Skip(1)]);
        }
    }
}
