using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Validator;
using Frostware.Pipe;
using MalFunction.Result;

namespace Frostscript
{
    public class Frostscript(IInterpreter interpreter)
    {
        public void Run(string frostscript)
        {
            var validationResult = 
                Lexer.Lex(frostscript)
                .Pipe(Parser.Parse)
                .MapFail(errors => errors.Select(error => new ValidationError (error.Token, error.Message)).ToArray())
                .Bind(Validator.Validate)
                .MapFail(x =>
                    x.Aggregate("", (errorMessage, newError) => errorMessage + $"[{newError.Token.Line}:{newError.Token.Character}] {newError.Error} \n")
                );

            if (validationResult is IResult<IExpression[], string>.Pass pass)
                pass.Value.Pipe(interpreter.Interpret);

            else if (validationResult is IResult<IExpression[], string>.Fail fail)
                throw new FrostscriptException(fail.Value);

            else 
                throw new Exception("Invalid result type");
        }
    }
}
