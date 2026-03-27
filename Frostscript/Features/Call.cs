using Frostscript.Internal;
using Frostscript.Types;

namespace Frostscript.Features
{
    internal class Call(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression node, IDictionary<string, object> variables)
        {
            if (node is CallExpression call)
            {
                var left = (ICallable)Next.Interpret(call.Left, variables);
                var right = Next.Interpret(call.Right, variables);

                return left.Call(right);
            }

            return Next.Interpret(node, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            (INode, Token[]) GenerateCall(INode node, Token[] tokens)
            {
                if (tokens.Length == 0)
                    return (node, tokens);

                if (tokens[0].Type is not (TokenType.SemiColon or TokenType.ParenthesesClose))
                {
                    var (argument, argumentTokens) = Next.Parse(tokens);
                    return GenerateCall(new CallNode(node, argument, tokens[0]), argumentTokens);
                }

                return (node, tokens[0].Type is TokenType.SemiColon ? [.. tokens.Skip(1)] : tokens);
            }

            var (left, leftTokens) = Next.Parse(tokens);
            return GenerateCall(left, leftTokens);
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
                                FunctionType functionType => (functionType.Parameter == right.DataType) switch
                                {
                                    true => new Pass(new CallExpression(left, right, functionType.Body)),
                                    false => new Fail(
                                        call.Token,
                                        $"Function expected a perameter of type {functionType.Parameter} but was given {right.DataType} instead"
                                    )
                                },
                                _ => new Fail(
                                    call.Token,
                                    $"{left.DataType} is not callable"
                                )
                            };
                        })
                    );
            }
            else return Next.Validate(node, variables);
        }
    }
}
