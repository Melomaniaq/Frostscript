using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Variable(IExpression Next) : IExpression
    {
        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Let or TokenType.Var)
            {
                if (tokens[1].Type is not TokenType.Label)
                    return (new ErrorNode($"Expected Label", tokens[1]), []);

                if (tokens[2].Type is not TokenType.SingleEqual)
                    return (new ErrorNode($"Expected '='", tokens[2]), []);

                var (value, valueTokens) = Next.Parse([.. tokens.Skip(3)]);
                return (new VariableNode(tokens[1].Literal, value, tokens[0].Type is TokenType.Var), valueTokens);
            }
            else return Next.Parse(tokens);
        }

        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            if (node is VariableNode variableNode)
            {
                variables[variableNode.Label] = variableNode.Value;
                return new Void();
            }
            else return Next.Interpret(node, variables);
        }
    }
}
