using Frostscript.Internal;

namespace Frostscript.Features
{
    internal class Literal : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is LiteralNode literal) return literal.Value;
            if (expression is ErrorNode error) throw new Exception($"Unhandled Parsing Error: {error.Error}");
            else throw new NotImplementedException("Node Could not be resolved. Did you forget to add the expression to the expression tree?");
        }
        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type == TokenType.Literal) return new(
                new LiteralNode(tokens[0].Literal, tokens[0]),
                [.. tokens.Skip(1)]
            );

            else return new(
                new ErrorNode($"Unexpected token {tokens[0].Literal}", tokens[0]),
                [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]
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

                return new Pass(new LiteralExpression(literal, dataType));
            }
            else throw new NotImplementedException("Node could not be resolved. Did you forget to add the expression to the expression tree?");
        }
    }
}
