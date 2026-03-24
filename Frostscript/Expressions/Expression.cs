using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal static class Expression
    {
        private static readonly IExpression expressions =
            new Variable(
            new Binary(BinaryType.Inequality,
            new Binary(BinaryType.Equality,
            new Binary(BinaryType.GreaterThan,
            new Binary(BinaryType.GreaterOrEqual,
            new Binary(BinaryType.LessThan,
            new Binary(BinaryType.LessOrEqual,
            new Binary(BinaryType.Addition,
            new Binary(BinaryType.Subtraction,
            new Binary(BinaryType.Devision,
            new Binary(BinaryType.Multiplication,
            new Label(
            new Literal()))))))))))));

        public static IExpression Expressions => expressions;
    }
}
