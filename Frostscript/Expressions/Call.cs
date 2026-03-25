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
            var (left, leftTokens) = Next.Parse(tokens);
            if (leftTokens.Length == 0) return (left, leftTokens);

            if (leftTokens.Length != 0 && leftTokens[0].Type is not TokenType.SemiColon)
            {
                var (right, rightTokens) = Next.Parse(leftTokens);
                return (new CallNode(left, right), rightTokens);
            }
           
            return (left, leftTokens[0].Type is TokenType.SemiColon ? [.. leftTokens.Skip(1)] : leftTokens);
        }
    }
}
