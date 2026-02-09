using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal static class Expression
    {
        private static readonly IExpression expressions = 
            new Binary([TokenType.Plus, TokenType.Minus], 
                new Binary([TokenType.ForwardSlash, TokenType.Star], 
                    new Literal(
                        new Error()
                    )
                )
            );
        public static IExpression Expressions => expressions;
    }
}
