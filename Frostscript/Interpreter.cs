using Frostscript.Expressions;
using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frostscript
{
    internal static class Interpreter
    {
        internal static T Interpret<T>(INode[] ast, IExpression expressions)
        {
            return ast
                .Select(x => (T)expressions.Interpret(x))
                .Last();
        }
    }
}
