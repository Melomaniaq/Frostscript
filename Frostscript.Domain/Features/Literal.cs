using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parser;
using Frostscript.Domain.Validator;

namespace Frostscript.Domain.Features
{
    public class Literal : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is LiteralExpression literal) return literal.Value;
            else throw new NotImplementedException("Node Could not be resolved. Did you forget to add the expression to the expression tree?");
        }
        public IParseResult Parse(Token[] tokens)
        {
            if (tokens[0].Type == TokenType.Literal) return new IParseResult.Pass(new (
                new LiteralNode(tokens[0].Literal, tokens[0]),
                [.. tokens.Skip(1)]
            ));

            else return new IParseResult.Fail([
                new ParseError(
                    tokens[0],
                    $"Unexpected token {tokens[0].Literal}",
                    tokens
                )]
            );
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is LiteralNode literal)
            {
                IDataType dataType = literal.Value switch
                {
                    sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or float or double or decimal => new NumberType(),
                    string or char => new StringType(),
                    bool => new BoolType(),
                    _ => throw new NotSupportedException()
                };

                return new IValidationResult.Pass(new TypedLiteralNode(literal.Value, dataType));
            }
            if (node is ErrorNode error) return new IValidationResult.Fail(new (error.Token, error.Error));
            else throw new NotImplementedException("Node could not be resolved. Did you forget to add the expression to the expression tree?");
        }
    }
}
