using Frostscript.Internal;
using Frostscript.Types;

namespace Frostscript.Features
{
    internal class Function(IFeature Next) : IFeature
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

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is not TokenType.Fun)
                return Next.Parse(tokens);

            try
            {
                var parameterTokens = ParameterList.TryParse([.. tokens.Skip(1)], out var parameters).ToArray();

                if (parameterTokens[0].Type is not TokenType.Arrow)
                    return (new ErrorNode("Expected '->' ", parameterTokens[0]), [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

                var (body, bodyTokens) = ExpressionTree.Parse([.. parameterTokens.Skip(1)]);
                return (new FunctionNode(parameters, body, tokens[0]), bodyTokens);

            }
            catch (Exception e) 
            {
                return (new ErrorNode(e.Message, tokens[0]), [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);
            }
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is FunctionNode function)
            {
                var closure = new Closure<string, VariableData>(variables);

                foreach (var (label, dataType) in function.Parameters)
                    closure[label] = new VariableData(dataType, false);

                return Validate(function.Body, closure)
                    .Map(body =>
                    {
                        var functionType = function.Parameters
                           .Reverse()
                           .Skip(1)
                           .Aggregate(
                               new FunctionType(function.Parameters.Last().dataType, body.DataType),
                               (frostFunc, parameter) => new FunctionType(
                                   parameter.dataType,
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
