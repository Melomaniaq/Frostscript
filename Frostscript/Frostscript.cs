using Frostscript.Features;
using Frostware.Pipe;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            return Lexer.Lex(frostscript)
                .Pipe(tokens => Parser.Parse(tokens, ExpressionTree.Global))
                .Pipe(nodes => Interpreter.Interpret<T>(nodes, ExpressionTree.Global));
        }

        public static void Run(string frostscript)
        {
            Lexer.Lex(frostscript)
            .Pipe(tokens => Parser.Parse(tokens, ExpressionTree.Global))
            .Pipe(nodes => Interpreter.Interpret(nodes, ExpressionTree.Global));
        }
    }
}
