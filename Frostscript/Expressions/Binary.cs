using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Binary(TokenType[] types, IExpression next) : IExpression
    {
        public (INode, Token[]) Parse(INode node, Token[] tokens)
        {
            var (left, tokensAfterLeft) = next.Parse(node, tokens);
            if (tokensAfterLeft.Length == 0) return (left, tokensAfterLeft);

            var @operator = tokensAfterLeft[0];
            if (types.Contains(@operator.Type))
            {
                var (right, tokensAfterRight) = next.Parse(node, [.. tokensAfterLeft.Skip(1)]);
                var operatorType = @operator.Type switch
                {
                    TokenType.Plus => BinaryType.Addition,
                    TokenType.Minus => BinaryType.Subtraction,
                    TokenType.Star => BinaryType.Multiplication,
                    TokenType.ForwardSlash => BinaryType.Devision
                };
                return (new BinaryNode(operatorType, left, right), tokensAfterRight);
            }
            else return (left, tokensAfterLeft);

        }
        public dynamic Interpret(INode node)
        {
            if (node is BinaryNode binary)
            {
                var left = next.Interpret(binary.Left);
                var right = next.Interpret(binary.Right);

                return binary.Type switch 
                { 
                    BinaryType.Addition => left + right,
                    BinaryType.Subtraction => left - right,
                    BinaryType.Multiplication => left * right,
                    BinaryType.Devision => left / right,
                };
            }
            else return next.Interpret(node);
        }
    }
}
