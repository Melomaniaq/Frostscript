using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parser;
using Frostscript.Domain.Types;
using Frostscript.Domain.Validator;
using MalFunction.Result;

namespace Frostscript.Domain.Features
{
    public class Assignment(IFeature Next) : IFeature
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

        public bool TryToDevide(int number1, int number2, out float result)
        {
            if (number2 == 0)
            {
                result = 0;
                return false;
            }
            else
            {
                result = number1 / number2;
                return true;
            }
        }
        public ParseResult Parse(Token[] tokens)
        {
            if (tokens.Length > 1 && tokens[0].Type is TokenType.Label && tokens[1].Type is TokenType.SingleEqual)
            {
                return Next.Parse([.. tokens.Skip(2)])
                    .Map(result => new ParseSuccess(
                        new AssignmentNode(tokens[0].Literal, result.Node, tokens[1]),
                        result.RemainingTokens
                    ));
            }

            else return Next.Parse(tokens);
        }

        public ValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is AssignmentNode assignment)
            {
                if (!variables.TryGetValue(assignment.Label, out var variableData))
                    return new ValidationResult.Fail(new (assignment.Token, $"Variable {assignment.Label} does not exist within scope"));

                if (!variableData.Mutable)
                    return new ValidationResult.Fail(new (assignment.Token, $"Variable {assignment.Label} is immutable and cannot be assigned to"));

                return Next.Validate(assignment.Value, variables)
                    .Bind(value =>
                    {
                        if (value.DataType.Equals(variableData.DataType)) 
                            return new ValidationResult.Pass(new TypedAssignmentNode(assignment.Label, value, new VoidType())) as ValidationResult;
                        else 
                            return new ValidationResult.Fail(new (
                            assignment.Token,
                            $"Variable {assignment.Label} is of type {variableData.DataType} and cannot be assigned a value of type {value.DataType}"
                        ));
                    });
            }

            else return Next.Validate(node, variables);
        }
    }
}
