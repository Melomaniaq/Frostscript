using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Validator;
using Frostware.Pipe;
using MalFunction.Result;

namespace Frostscript
{
    public static class Frostscript
    {
        public static IResult<IExpression[], string> Build(string frostscript)
        {
            return Lexer.Lex(frostscript)
                .Pipe(Parser.Parse)
                .MapFail(errors => errors.Select(error => new ValidationError (error.Token, error.Message)).ToArray())
                .Bind(Validator.Validate)
                .MapFail(x =>
                    x.Aggregate("", (errorMessage, newError) => errorMessage + $"[{newError.Token.Line}:{newError.Token.Character}] {newError.Error} \n")
                );
        }
    }
}
