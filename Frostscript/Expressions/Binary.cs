using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Binary(BinaryType type, IExpression next) : IExpression
    {
        readonly Dictionary<BinaryType, TokenType> operatorMap = new() 
        { 
            { BinaryType.Addition, TokenType.Plus }, 
            { BinaryType.Subtraction, TokenType.Minus }, 
            { BinaryType.Multiplication, TokenType.Star }, 
            { BinaryType.Devision, TokenType.ForwardSlash }, 
            { BinaryType.Equality, TokenType.DoubleEqual }, 
            { BinaryType.Inequality, TokenType.NotEqual }, 
            { BinaryType.GreaterThan, TokenType.GreaterThan }, 
            { BinaryType.GreaterOrEqual, TokenType.GreaterOrEqual }, 
            { BinaryType.LessThan, TokenType.LessThan }, 
            { BinaryType.LessOrEqual, TokenType.LessOrEqual },
            { BinaryType.And, TokenType.And },
            { BinaryType.Or, TokenType.Or },
        };
        public (INode, Token[]) Parse(Token[] tokens)
        {
            var (left, tokensAfterLeft) = next.Parse(tokens);
            if (tokensAfterLeft.Length == 0) return (left, tokensAfterLeft);

            if (operatorMap[type] == tokensAfterLeft[0].Type)
            {
                var (right, tokensAfterRight) = next.Parse([.. tokensAfterLeft.Skip(1)]);
                return (new BinaryNode(type, left, right), tokensAfterRight);
            }
            else return (left, tokensAfterLeft);

        }
        public dynamic Interpret(INode node, Dictionary<string, INode> variables)
        {
            if (node is BinaryNode binary)
            {
                var left = next.Interpret(binary.Left, variables);
                var right = next.Interpret(binary.Right, variables);

                return binary.Type switch 
                { 
                    BinaryType.Addition => left + right,
                    BinaryType.Subtraction => left - right,
                    BinaryType.Multiplication => left * right,
                    BinaryType.Devision => left / right,
                    BinaryType.Equality => left == right,
                    BinaryType.Inequality => left != right,
                    BinaryType.GreaterThan => left > right,
                    BinaryType.GreaterOrEqual => left >= right,
                    BinaryType.LessThan => left < right,
                    BinaryType.LessOrEqual => left <= right,
                    BinaryType.And => left && right,
                    BinaryType.Or => left || right,
                };
            }
            else return next.Interpret(node, variables);
        }
    }
}
