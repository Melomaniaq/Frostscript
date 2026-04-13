using Frostscript.Domain.Features;
using Frostscript.Domain.Internal;

namespace Frostscript
{
    internal static class Interpreter
    {
        internal static dynamic Interpret(IExpression[] ast)
        {
            VariableDictionary globalVariables = [];

            return ast
                .Select(x => ExpressionTree.Interpret(x, globalVariables))
                .ToArray()
                .Last();
        }

        internal static T Interpret<T>(IExpression[] ast) => (T)Interpret(ast);

       
    }
}
