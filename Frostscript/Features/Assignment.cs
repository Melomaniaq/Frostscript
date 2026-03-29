using Frostscript.Internal;
using Frostscript.Types;

namespace Frostscript.Features
{
    internal class Assignment(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is AssignmentExpression assignment)
            {
                variables[assignment.Label] = Next.Interpret(assignment.Value, variables);
                return new FSVoid();
            }

            else return Next.Interpret(expression, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens.Length > 1 && tokens[0].Type is TokenType.Label && tokens[1].Type is TokenType.SingleEqual)
            {
                var (value, valueTokens) = Next.Parse([.. tokens.Skip(2)]);
                return (
                    new AssignmentNode(tokens[0].Literal, value, tokens[1]),
                    valueTokens
                );
            }

            else return Next.Parse(tokens);
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is AssignmentNode assignment)
            {
                if (!variables[assignment.Label].Mutable)
                    return new Fail(assignment.Token, $"Variable {assignment.Label} is immutable and cannot be assigned to");

                if (!variables.TryGetValue(assignment.Label, out var variableData))
                    return new Fail(assignment.Token, $"Variable {assignment.Label} does not exist within scope");

                return Next.Validate(assignment.Value, variables)
                    .Bind(value =>
                    {
                        if (value.DataType == variableData.DataType) return new Pass(new AssignmentExpression(assignment.Label, value, new VoidType()));
                        else return new Fail(
                            assignment.Token,
                            $"Variable {assignment.Label} is of type {variableData.DataType} and cannot be assigned a value of type {value.DataType}"
                        );
                    });
            }

            else return Next.Validate(node, variables);
        }
    }
}
