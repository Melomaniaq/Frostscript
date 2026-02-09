using Frostscript.Expressions;
using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Frostscript.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void LiteralString()
        {
            INode[] nodes = [new LiteralNode("Hello")];
            var expression = new Literal(new Error());
            Assert.Equal("Hello", Interpreter.Interpret<string>(nodes, expression));
        }

        [Theory]
        [InlineData(BinaryType.Addition, TokenType.Plus, 6)]
        [InlineData(BinaryType.Subtraction, TokenType.Minus, 2)]
        [InlineData(BinaryType.Devision, TokenType.ForwardSlash, 2)]
        [InlineData(BinaryType.Multiplication, TokenType.Star, 8)]
        internal void Binary(BinaryType type, TokenType @operator, int expected)
        {
            INode[] nodes = [new BinaryNode(type, new LiteralNode(4), new LiteralNode(2))];
            var expression = new Binary([@operator], new Literal(new Error()));
            Assert.Equal(expected, Interpreter.Interpret<int>(nodes, expression));
        }
    }
}
