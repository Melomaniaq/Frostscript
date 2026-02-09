using Frostscript.Expressions;
using Frostware.Pipe;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            return Lexer.Lex(frostscript)
                .Pipe(tokens => Parser.Parse(tokens, Expression.Expressions))
                .Pipe(nodes => Interpreter.Interpret<T>(nodes, Expression.Expressions));
        }
    }
}
