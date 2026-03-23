using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal static class Expression
    {
        private static readonly IExpression expressions =
            new Binary(BinaryType.Inequality,
            new Binary(BinaryType.Equality,
            new Binary(BinaryType.Addition,
            new Binary(BinaryType.Subtraction,
            new Binary(BinaryType.Devision,
            new Binary(BinaryType.Multiplication,
            new Literal(new Error())))))));

        public static IExpression Expressions => expressions;
    }
}
