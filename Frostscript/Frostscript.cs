using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Validator;
using Frostware.Pipe;
using MalFunction.Result;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            var validationResult = 
                Lexer.Lex(frostscript)
                .Pipe(Parser.Parse)
                .MapFail(errors => errors.Select(error => new ValidationError (error.Token, error.Message)).ToArray())
                .Bind(Validator.Validate)
                .MapFail(x =>
                    x.Aggregate("", (errorMessage, newError) => errorMessage + $"[{newError.Token.Line}:{newError.Token.Character}] {newError.Error} \n")
                );

            return validationResult switch
            {
                IResult<IExpression[], string>.Pass pass => pass.Value
                    .Pipe(Interpreter.Interpret<T>),
                IResult<IExpression[], string>.Fail fail => throw new FrostscriptException(fail.Value),
                _ => throw new Exception("Invalid result type")
            };
        }

        public static void Run(string frostscript) => Run<object>(frostscript);
    }
}
