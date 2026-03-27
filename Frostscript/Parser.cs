using Frostscript.Expressions;
using Frostscript.Internal;
using System.Linq;

namespace Frostscript
{
    internal static class Parser
    {
        public static IExpression[] Parse(Token[] tokens, IFeature features)
        {
            IExpression[] GenerateNodes(IExpression[] nodes, Token[] tokens)
            {
                if (tokens.Length > 0)
                {
                    var (node, newTokens) = features.Parse(tokens);
                    return GenerateNodes([.. nodes.Append(node)], newTokens);
                }
                else return nodes;
            }

            return GenerateNodes([], tokens);
        }
    }
}
