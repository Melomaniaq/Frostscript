using Frostscript.Domain.Features;
using Frostscript.Domain.Internal;
using Frostware.Pipe;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            var validationResult = Lexer.Lex(frostscript)
                .Pipe(Parser.Parse)
                .Pipe(Validator.Validate);

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
