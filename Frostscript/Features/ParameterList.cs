using Frostscript.Internal;

namespace Frostscript.Features
{
    internal class ParameterList
    {
        public static Token[] TryParse(Token[] tokens, out (string label, IDataType type)[] parameters)
        {
            var _tokens = tokens;

            IEnumerable<(string label, IDataType dataType)> GenerateParameters()
            {
                while (_tokens[0].Type != TokenType.Arrow)
                {
                    if (_tokens[0].Type is not TokenType.Label)
                        throw new Exception("Expected parameter");

                    var annotation = Annotation([.. _tokens.Skip(1)]);
                    _tokens = annotation.tokens;
                    yield return (_tokens[0].Literal, annotation.data);
                }
            }
            parameters = [.. GenerateParameters()];
            return _tokens;

        }
        static (IDataType data, Token[] tokens) Annotation(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.ParenthesesOpen)
            {
                return FunctionType([.. tokens.Skip(1)]);
            }
            else return (new UnknownType(), tokens);
        }

        static (IDataType dataType, Token[] tokens) FunctionType(Token[] tokens)
        {
            var parameter = Type(tokens);
            if (parameter.tokens[0].Type is TokenType.Arrow)
            {
                var body = FunctionType([.. parameter.tokens.Skip(1)]);
                if (body.dataType is FunctionType func)
                    return (new FunctionType(parameter.dataType, func), body.tokens);
                else return (new FunctionType(parameter.dataType, body.dataType), body.tokens);
            }
            else return parameter;
        }

        static (IDataType dataType, Token[] tokens) Type(Token[] tokens)
        {
            return tokens[0].Type switch
            {
                TokenType.Num => (new NumberType(), [.. tokens.Skip(1)]),
                TokenType.Str => (new StringType(), [.. tokens.Skip(1)]),
                TokenType.Bool => (new BoolType(), [.. tokens.Skip(1)]),
                _ => throw new Exception("Expected type after '('")
            };
        }
    }
}
