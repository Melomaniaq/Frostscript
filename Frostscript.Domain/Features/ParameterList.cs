using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parameters;
using Frostscript.Domain.Parser;
using MalFunction.Result;
using AnnotationResult = MalFunction.Result.IResult<
    (Frostscript.Domain.Features.Models.IDataType dataType, Frostscript.Domain.Token[] remainingTokens),
    Frostscript.Domain.Parser.ParseError
>;

namespace Frostscript.Domain.Features
{
    public static class ParameterList
    {
        public static IResult<ParameterListSuccess, ParseError[]> Parse(Token[] tokens)
        {
            var _tokens = tokens;

            IEnumerable<IResult<Parameter, ParseError>> GenerateParameters()
            {
                while (_tokens.Length != 0 && _tokens[0].Type is not TokenType.Arrow)
                {
                    if (_tokens[0].Type is not TokenType.Label)
                    {
                        var error = new ParseError(_tokens[0], "Expected parameter", _tokens);
                        _tokens = error.RemainingTokens;
                        yield return new IResult<Parameter, ParseError>.Fail(error);
                        continue;
                    }

                    var label = _tokens[0].Literal;
                    var annotation = Annotation([.. _tokens.Skip(1)]);

                    if (annotation is AnnotationResult.Fail fail)
                        _tokens = fail.Value.RemainingTokens;
                    else if (annotation is AnnotationResult.Pass pass)
                        _tokens = pass.Value.remainingTokens;

                    yield return annotation.Map(annotation => new Parameter(label, annotation.dataType));
                }
            }
            return GenerateParameters()
                .ToArray()
                .Traverse()
                .Map(parameterList => new ParameterListSuccess(parameterList, _tokens));
        }
        static AnnotationResult Annotation(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.ParenthesesOpen)
            {
                return FunctionType([.. tokens.Skip(1)]).Bind(annotation =>
                {
                    if (annotation.remainingTokens[0].Type is not TokenType.ParenthesesClose)
                        return new AnnotationResult.Fail(new ParseError(annotation.remainingTokens[0], "Expected ')'", annotation.remainingTokens)) as AnnotationResult;
                    else 
                        return new AnnotationResult.Pass((annotation.dataType, [.. annotation.remainingTokens.Skip(1)]));

                });
            }
            else return new AnnotationResult.Pass((new UnknownType(), tokens));
        }

        static AnnotationResult FunctionType(Token[] tokens)
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
                else return new AnnotationResult.Pass(parameter);
            });
        }

        static AnnotationResult Type(Token[] tokens)
        {
            return tokens[0].Type switch
            {
                TokenType.Num => new AnnotationResult.Pass((new NumberType(), [.. tokens.Skip(1)])),
                TokenType.Str => new AnnotationResult.Pass((new StringType(), [.. tokens.Skip(1)])),
                TokenType.Bool => new AnnotationResult.Pass((new BoolType(), [.. tokens.Skip(1)])),
                _ => new AnnotationResult.Fail(new ParseError(tokens[0], "Expected type after '('", tokens))
            };
        }
    }
}
