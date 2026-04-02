using Frostscript.Features;
using Frostscript.Internal;
using System.Linq;

namespace Frostscript
{
    internal static class Parser
    {
        public static INode[] Parse(Token[] tokens)
        {
            INode[] GenerateNodes(INode[] nodes, Token[] tokens)
            {
                if (tokens.Length > 0)
                {
                    var (node, newTokens) = ExpressionTree.Parse(tokens);
                    return GenerateNodes([.. nodes.Append(node)], newTokens);
                }
                else return nodes;
            }

            return GenerateNodes([], tokens);
        }
    }
}
