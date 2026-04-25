using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parser;
using Frostscript.Domain.Types;
using Frostscript.Domain.Validator;
using MalFunction.Result;

namespace Frostscript.Domain.Features
{
    public class VariableDecleration(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is VariableExpression variable)
            {
                variables[variable.Label] = Next.Interpret(variable.Value, variables);
                return new FSVoid();
            }
            else return Next.Interpret(expression, variables);
        }
        public ParseResult Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Let or TokenType.Var)
            {
                if (tokens[1].Type is not TokenType.Label)
                    return new ParseResult.Fail([new ParseError(tokens[1], "Expected Label", tokens)]);
              
                if (tokens[2].Type is not TokenType.SingleEqual)
                    return new ParseResult.Fail([new ParseError(tokens[2], "Expected '='", tokens)]);

                return Next.Parse([.. tokens.Skip(3)])
                    .Map(value => new ParseSuccess(
                        new VariableNode(tokens[1].Literal, value.Node, tokens[0].Type is TokenType.Var, tokens[0]), 
                        value.RemainingTokens
                    ));
            }
            else return Next.Parse(tokens);
        }

        public ValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is VariableNode variable)
            {
                return Next.Validate(variable.Value, variables)
                    .Bind(value =>
                    {
                        variables[variable.Label] = new VariableData(value.DataType, variable.Mutable);
                        return new ValidationResult.Pass(new TypedVariableNode(variable.Label, value, variable.Mutable, new VoidType()));
                    });
            }
            else return Next.Validate(node, variables);
        }
    }
}
