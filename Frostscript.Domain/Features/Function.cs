using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parser;
using Frostscript.Domain.Types;
using Frostscript.Domain.Validator;

namespace Frostscript.Domain.Features
{
    public class Function(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is FunctionExpression function) 
                return function.Parameters
                .Reverse()
                .Skip(1)
                .Aggregate(
                    new FSFunction(function.Parameters.Last(), function.Body, new Closure<string, object>(variables)),
                    (frostFunc, parameter) => new FSFunction(
                        parameter, 
                        new LiteralExpression(frostFunc), 
                        frostFunc.Closure
                    )
                );

            else return Next.Interpret(expression, variables);
        }

        public IParseResult Parse(Token[] tokens)
        {
            if (tokens[0].Type is not TokenType.Fun)
                return Next.Parse(tokens);

            return ParameterList.Parse([.. tokens.Skip(1)]).Bind(parameterList =>
            {
                if (parameterList.RemainingTokens[0].Type is not TokenType.Arrow)
                    return new IParseResult.Fail([new ParseError(parameterList.RemainingTokens[0], "Expected '->' ", parameterList.RemainingTokens)]);

                return ExpressionTree.Parse([.. parameterList.RemainingTokens.Skip(1)]).Map(body =>
                    new ParseSuccess(new FunctionNode(parameterList.Parameters, body.Node, tokens[0]), body.RemainingTokens)
                );
            });
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is FunctionNode function)
            {
                var closure = new Closure<string, VariableData>(variables);

                foreach (var (label, dataType) in function.Parameters)
                    closure[label] = new VariableData(dataType, false);

                return ExpressionTree.Validate(function.Body, closure)
                    .Map(body =>
                    {
                        var functionType = function.Parameters
                           .Reverse()
                           .Skip(1)
                           .Aggregate(
                               new FunctionType(function.Parameters.Last().DataType, body.DataType),
                               (frostFunc, parameter) => new FunctionType(
                                   parameter.DataType,
                                   frostFunc
                               )
                            );

                        return new TypedFunctionNode(function.Parameters, body, functionType) as ITypedNode;
                    });
            }
            else return Next.Validate(node, variables);
        }
    }
}
