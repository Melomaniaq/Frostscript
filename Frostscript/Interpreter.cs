using Frostscript.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frostscript
{
    internal static class Interpreter
    {
        internal static dynamic Interpret(INode[] ast, IExpression expressions)
        {
            VariableDictionary globalVariables = [];

            return ast
            .Select(x => expressions.Interpret(x, globalVariables))
            .ToArray()
            .Last();
        }

        internal static T Interpret<T>(INode[] ast, IExpression expressions) => (T)Interpret(ast, expressions);
      
    }
}
