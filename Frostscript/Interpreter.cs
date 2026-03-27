using Frostscript.Expressions;
using Frostscript.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frostscript
{
    internal static class Interpreter
    {
        internal static dynamic Interpret(IExpression[] ast, IFeature expressions)
        {
            VariableDictionary globalVariables = [];

            return ast
            .Select(x => expressions.Interpret(x, globalVariables))
            .ToArray()
            .Last();
        }

        internal static T Interpret<T>(IExpression[] ast, IFeature expressions) => (T)Interpret(ast, expressions);
    }
}
