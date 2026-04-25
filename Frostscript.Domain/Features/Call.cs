using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Types;
using Frostscript.Domain.Validator;
using MalFunction.Result;

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

        public ParseResult Parse(Token[] tokens)
        {
            return Next.Parse(tokens).Bind(left =>
            {
                ParseResult GenerateCall(INode node, Token[] tokens)
                {
                    if (tokens.Length == 0)
                        return new ParseResult.Pass(new (node, tokens));

                    if (tokens[0].Type is not (TokenType.ParenthesesClose or TokenType.SemiColon))
                    {
                        return Next.Parse(tokens)
                            .Bind(argument => GenerateCall(new CallNode(node, argument.Node, left.Node.Token), argument.RemainingTokens));
                    }

                    return new ParseResult.Pass(new (node, tokens[0].Type is TokenType.SemiColon ? [.. tokens.Skip(1)] : tokens));
                }

                return GenerateCall(left.Node, left.RemainingTokens);
                });
        }

        public ValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
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
                                    true => new ValidationResult.Pass(new TypedCallNode(left, right, functionType.Body)) as ValidationResult,
                                    false => new ValidationResult.Fail(new (
                                        call.Token,
                                        $"Function expected a argument of type {functionType.Parameter} but was given {right.DataType} instead"
                                    ))
                                },
                                _ => new ValidationResult.Fail(new (
                                    call.Token,
                                    $"{left.DataType} is not callable, are you missing a ';'?"
                                ))
                            };
                        })
                    );
            }
            else return Next.Validate(node, variables);
        }
    }
}
