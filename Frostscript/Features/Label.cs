using Frostscript.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Label(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression node, IDictionary<string, object> variables)
        {
            if (node is LabelNode label) 
                return variables[label.Label];
            else 
                return Next.Interpret(node, variables);
        }

        public (IExpression, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Label) 
                return (new LabelNode(tokens[0].Literal), [.. tokens.Skip(1)]);
            else 
                return Next.Parse(tokens);
        }
    }
}
