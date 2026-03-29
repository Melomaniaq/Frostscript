using Frostscript.Internal;
using Frostscript.Types;

namespace Frostscript.Features
{
    internal class VariableDecleration(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression node, IDictionary<string, object> variables)
        {
            if (node is VariableNode variableNode)
            {
                variables[variableNode.Label] = ExpressionTree.Interpret(variableNode.Value, variables);
                return new FSVoid();
            }
            else return Next.Interpret(node, variables);
        }
        public ParserResult Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Let or TokenType.Var)
            {
                if (tokens[1].Type is not TokenType.Label)
                    return (
                        new NodeContext(new ErrorNode($"Expected Label"), tokens[1]),
                        [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]
                    );
                return new ParserResult(
                    Node: new ErrorNode($"Expected Label"),
                    Token: tokens[1],
                    RemainingTokens: [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]
                );

                if (tokens[2].Type is not TokenType.SingleEqual)
                    return (new ErrorNode($"Expected '='", tokens[2]), [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

                var (value, valueTokens) = Next.Parse([.. tokens.Skip(3)]);
                return (new VariableNode(tokens[1].Literal, value, tokens[0].Type is TokenType.Var, tokens[1]), valueTokens);
            }
            else return Next.Parse(tokens);
        }
    }
}
