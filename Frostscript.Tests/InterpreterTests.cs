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
        [InlineData([BinaryType.Addition, 1, 2, 3])]
        [InlineData([BinaryType.Subtraction, 1, 2, -1])]
        [InlineData([BinaryType.Multiplication, 2, 3, 6])]
        [InlineData([BinaryType.Devision, 6, 3, 2])]
        [InlineData([BinaryType.Equality, 1, 1, true])]
        [InlineData([BinaryType.Equality, 1, 2, false])]
        [InlineData([BinaryType.Inequality, 1, 1, false])]
        [InlineData([BinaryType.Inequality, 1, 2, true])]
        internal void Binary(BinaryType type, dynamic left, dynamic right, dynamic result)
        {
            INode[] nodes = [new BinaryNode(type, new LiteralNode(left), new LiteralNode(right))];
            var expression = new Binary(type, new Literal(new Error()));
            Assert.Equal(result, Interpreter.Interpret<dynamic>(nodes, expression));
        }
    }
}
