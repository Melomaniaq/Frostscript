using Frostscript.Types;

namespace Frostscript.Expressions
{
    internal class Assignment(IExpression Next) : IExpression
    {
        public dynamic Interpret(INode node, IDictionary<string, INode> variables)
        {
            if (node is AssignmentNode assignment)
            {
                variables[assignment.Label] = assignment.Value;
                return new FSVoid();
            }
            else return Next.Interpret(node, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens.Length > 1 && tokens[0].Type is TokenType.Label && tokens[1].Type is TokenType.SingleEqual)
            {
                var (value, valueTokens) = Next.Parse([.. tokens.Skip(2)]);
                return (new AssignmentNode(tokens[0].Literal, value), valueTokens);
            }
            else return Next.Parse(tokens);
        }
    }
}
