using Frostscript.Domain.Internal;
using IAnnotationResult = Frostscript.Domain.Internal.IResult<
    (Frostscript.Domain.Internal.IDataType dataType, Frostscript.Domain.Internal.Token[] remainingTokens), 
    Frostscript.Domain.Internal.ParseError
>;
using IParameterListResult = Frostscript.Domain.Internal.IResult<
    Frostscript.Domain.Internal.ParameterListSuccess,
    Frostscript.Domain.Internal.ParseError[]
>;

namespace Frostscript.Domain.Features
{
    public static class ParameterList
    {
        public static IParameterListResult Parse(Token[] tokens)
        {
            var _tokens = tokens;

            IEnumerable<IResult<Parameter, ParseError>> GenerateParameters()
            {
                while (_tokens[0].Type != TokenType.Arrow)
                {
                    if (_tokens[0].Type is not TokenType.Label)
                        throw new Exception("Expected parameter");
                    var label = _tokens[0].Literal;
                    var annotation = Annotation([.. _tokens.Skip(1)]);

                    if (annotation is IAnnotationResult.Fail fail)
                        _tokens = fail.Value.RemainingTokens;
                    else if (annotation is IAnnotationResult.Pass pass)
                        _tokens = pass.Value.remainingTokens;

                    yield return annotation.Map(annotation => new Parameter(label, annotation.dataType));
                }
            }
            return GenerateParameters()
                .Traverse()
                .Map(parameterList => new ParameterListSuccess(parameterList, _tokens));
        }
        static IAnnotationResult Annotation(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.ParenthesesOpen)
            {
                return FunctionType([.. tokens.Skip(1)]).Bind(annotation =>
                {
                    if (annotation.remainingTokens[0].Type is not TokenType.ParenthesesClose)
                        return new IAnnotationResult.Fail(new ParseError(annotation.remainingTokens[0], "Expected ')'", annotation.remainingTokens)) as IAnnotationResult;
                    else 
                        return new IAnnotationResult.Pass((annotation.dataType, [.. annotation.remainingTokens.Skip(1)]));

                });
            }
            else return new IAnnotationResult.Pass((new UnknownType(), tokens));
        }

        static IAnnotationResult FunctionType(Token[] tokens)
        {
            return Type(tokens).Bind(parameter =>
            {
                if (parameter.remainingTokens[0].Type is TokenType.Arrow)
                {
                    return FunctionType([.. parameter.remainingTokens.Skip(1)]).Map(body =>
                    {
                        if (body.dataType is FunctionType func)
                            return (new FunctionType(parameter.dataType, func) as IDataType, body.remainingTokens);
                        else 
                            return (new FunctionType(parameter.dataType, body.dataType), body.remainingTokens);
                    });
                }
                else return new IAnnotationResult.Pass(parameter);
            });
        }

        static IAnnotationResult Type(Token[] tokens)
        {
            return tokens[0].Type switch
            {
                TokenType.Num => new IAnnotationResult.Pass((new NumberType(), [.. tokens.Skip(1)])),
                TokenType.Str => new IAnnotationResult.Pass((new StringType(), [.. tokens.Skip(1)])),
                TokenType.Bool => new IAnnotationResult.Pass((new BoolType(), [.. tokens.Skip(1)])),
                _ => new IAnnotationResult.Fail(new ParseError(tokens[0], "Expected type after '('", tokens))
            };
        }
    }
}
