using Frostscript.Domain.Internal;

namespace Frostscript.Domain.Features
{
    public class Parentheses(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is ParenthesesExpression parentheses)
                return ExpressionTree.Interpret(parentheses.Body, variables);

            return Next.Interpret(expression, variables);
        }

        public IParseResult Parse(Token[] tokens)
        {
            if (tokens[0].Type is not TokenType.ParenthesesOpen)
                return Next.Parse(tokens);

            return ExpressionTree.Parse([.. tokens.Skip(1)]).Bind(body =>
            {
                if (body.RemainingTokens.Length != 0 && body.RemainingTokens[0].Type is not TokenType.ParenthesesClose)
                    return new IParseResult.Fail([new ParseError(body.RemainingTokens[0], "Expected ')'", body.RemainingTokens)]) as IParseResult;
                else 
                    return new IParseResult.Pass(new ParseSuccess(new ParenthesesNode(body.Node, tokens[0]), [.. body.RemainingTokens.Skip(1)]));
            });
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is ParenthesesNode parentheses)
            {
                return ExpressionTree.Validate(parentheses.Body, variables)
                    .Bind(body => new IValidationResult.Pass(new TypedParenthesesNode(body, body.DataType)));
            }
            else return Next.Validate(node, variables);
        }
    }
}
