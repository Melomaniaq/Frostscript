using Frostscript.Expressions;

namespace Frostscript
{
    public static class Frostscript
    {
        public static T Run<T>(string frostscript)
        {
            return Interpreter.Interpret<T>(Parser.Parse(Lexer.Lex(frostscript), Expression.Expressions), Expression.Expressions);
        }
    }
}
