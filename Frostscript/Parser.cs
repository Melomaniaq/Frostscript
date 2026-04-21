using Frostscript.Domain.Features;
using Frostscript.Domain.Internal;

namespace Frostscript
{
    internal static class Parser
    {
        public static IResult<INode[], ParseError[]> Parse(Token[] tokens)
        {
            IResult<INode[], ParseError[]> GenerateNodes(INode[] nodes, Token[] tokens)
            {
                if (tokens.Length > 0)
                    return ExpressionTree.Parse(tokens).Bind(node => GenerateNodes([.. nodes.Append(node.Node)], node.RemainingTokens));
                else return new IResult<INode[], ParseError[]>.Pass(nodes);
            }

            return GenerateNodes([], tokens);
        }
    }
}
