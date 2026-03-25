using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Label(IExpression Next) : IExpression
    {
        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            if (node is LabelNode label) 
                return Next.Interpret(variables[label.Label], variables);
            else 
                return Next.Interpret(node, variables);
        }

        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Label) 
                return (new LabelNode(tokens[0].Literal), [.. tokens.Skip(1)]);
            else 
                return Next.Parse(tokens);
        }
    }
}
