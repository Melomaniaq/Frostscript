using Frostscript.Internal;
using Frostscript.Types;

namespace Frostscript.Expressions
{
    internal class Assignment(IExpression Next) : IExpression
    {
        public dynamic Interpret(INode node, IDictionary<string, dynamic> variables)
        {
            if (node is AssignmentNode assignment)
            {
                variables[assignment.Label] = Expression.ExpressionTree.Interpret(assignment.Value, variables);
                return new FSVoid();
            }

            else return Next.Interpret(node, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens.Length > 1 && tokens[0].Type is TokenType.Label && tokens[1].Type is TokenType.SingleEqual)
            {
                var (value, valueTokens) = Next.Parse([.. tokens.Skip(2)]);
                return (
                    new AssignmentNode(tokens[0].Literal, value, new VoidType(), tokens[1]),
                    valueTokens
                );
            }

            else return Next.Parse(tokens);
        }

        public IValidationResult Validate(NodeContext context, IDictionary<string, ValidationVariable> variables)
        {
            if (context.Node is AssignmentNode assignment)
            {
                if (!variables[assignment.Label].Mutable)
                    return new Fail($"Variable {assignment.Label} is immutable and cannot be assigned to", context.Token);

                if (!variables.TryGetValue(assignment.Label, out var value))
                    return new Fail($"Variable {assignment.Label} does not exist within scope", context.Token);

                if (assignment.Value.DataType != value.DataType)
                    return new Fail(
                        $"Variable {assignment.Label} is of type {variables[assignment.Label].DataType} and cannot be assigned a value of type {assignment.Value.DataType}", 
                        context.Token
                    );

                return new Pass(assignment);
            }

            else return Next.Validate(context, variables);
        }
    }
}
