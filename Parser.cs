using Frostscript.Expressions;
using Frostscript.Nodes;
using System.Linq;

namespace Frostscript
{
    internal static class Parser
    {
        public static INode[] Parse(Token[] tokens, IExpression features)
        {
            INode[] GenerateNodes(INode[] nodes, Token[] tokens)
            {
                if (tokens.Length > 0)
                {
                    var (node, newTokens) = features.Parse(new StatementNode(), tokens);
                    return GenerateNodes([.. nodes.Append(node)], newTokens);
                }
                else return nodes;
            }

            return GenerateNodes([], tokens);
        }
    }
}
