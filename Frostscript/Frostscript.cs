using Frostscript.Expressions;
using Frostware.Pipe;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            return Lexer.Lex(frostscript)
                .Pipe(tokens => Parser.Parse(tokens, Expression.ExpressionTree))
                .Pipe(nodes => Interpreter.Interpret<T>(nodes, Expression.ExpressionTree));
        }

        public static void Run(string frostscript)
        {
            Lexer.Lex(frostscript)
            .Pipe(tokens => Parser.Parse(tokens, Expression.ExpressionTree))
            .Pipe(nodes => Interpreter.Interpret(nodes, Expression.ExpressionTree));
        }
    }
}
