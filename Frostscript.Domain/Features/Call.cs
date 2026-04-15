using Frostscript.Domain.Internal;
using Frostscript.Domain.Types;

namespace Frostscript.Domain.Features
{
    public class Call(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is CallExpression call)
            {
                var left = (ICallable)Next.Interpret(call.Left, variables);
                var right = Next.Interpret(call.Right, variables);

                return left.Call(right);
            }

            return Next.Interpret(expression, variables);
        }

        public IParseResult Parse(Token[] tokens)
        {
            return Next.Parse(tokens).Bind(left =>
            {
                IParseResult GenerateCall(INode node, Token[] tokens)
                {
                    if (tokens.Length == 0)
                        return new IParseResult.Pass(new (node, tokens));

                    if (tokens[0].Type is not (TokenType.ParenthesesClose or TokenType.SemiColon))
                    {
                        return Next.Parse(tokens)
                            .Bind(argument => GenerateCall(new CallNode(node, argument.Node, left.Node.Token), argument.RemainingTokens));
                    }

                    return new IParseResult.Pass(new (node, tokens[0].Type is TokenType.SemiColon ? [.. tokens.Skip(1)] : tokens));
                }

                return GenerateCall(left.Node, left.RemainingTokens);
                });
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is CallNode call)
            {
                return Validate(call.Left, variables)
                    .Bind(left => Validate(call.Right, variables)
                        .Bind(right =>
                        {
                            return left.DataType switch
                            {
                                FunctionType functionType => (functionType.Parameter.Equals(right.DataType)) switch
                                {
                                    true => new IValidationResult.Pass(new TypedCallNode(left, right, functionType.Body)) as IValidationResult,
                                    false => new IValidationResult.Fail((
                                        call.Token,
                                        $"Function expected a argument of type {functionType.Parameter} but was given {right.DataType} instead"
                                    ))
                                },
                                _ => new IValidationResult.Fail((
                                    call.Token,
                                    $"{left.DataType} is not callable"
                                ))
                            };
                        })
                    );
            }
            else return Next.Validate(node, variables);
        }
    }
}
