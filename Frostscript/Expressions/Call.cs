using Frostscript.Types;

namespace Frostscript.Expressions
{
    internal class Call(IExpression Next) : IExpression
    {
        public dynamic Interpret(INode node, IDictionary<string, INode> variables)
        {
            if (node is CallNode call)
            {
                var left = (ICallable)Next.Interpret(call.Left, variables);
                var right = Next.Interpret(call.Right, variables);

                return left.Call(right);
            }

            return Next.Interpret(node, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            (INode, Token[]) GenerateCall(INode node, Token[] tokens)
            {
                if (tokens.Length == 0)
                    return (node, tokens);

                if (tokens[0].Type is not (TokenType.SemiColon or TokenType.ParenthesesClose))
                {
                    var (argument, argumentTokens) = Next.Parse(tokens);
                    return GenerateCall(new CallNode(node, argument), argumentTokens);
                }

                return (node, tokens[0].Type is TokenType.SemiColon ? [.. tokens.Skip(1)] : tokens);
            }

            var (left, leftTokens) = Next.Parse(tokens);
            return GenerateCall(left, leftTokens);
        }
    }
}
