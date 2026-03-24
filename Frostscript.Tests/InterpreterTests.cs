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
            var expression = new Literal();
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

        [InlineData([BinaryType.GreaterThan, 1, 2, false])]
        [InlineData([BinaryType.GreaterThan, 2, 1, true])]
        [InlineData([BinaryType.GreaterThan, 1, 1, false])]

        [InlineData([BinaryType.GreaterOrEqual, 1, 2, false])]
        [InlineData([BinaryType.GreaterOrEqual, 2, 1, true])]
        [InlineData([BinaryType.GreaterOrEqual, 1, 1, true])]

        [InlineData([BinaryType.LessThan, 1, 2, true])]
        [InlineData([BinaryType.LessThan, 2, 1, false])]
        [InlineData([BinaryType.LessThan, 1, 1, false])]

        [InlineData([BinaryType.LessOrEqual, 1, 2, true])]
        [InlineData([BinaryType.LessOrEqual, 2, 1, false])]
        [InlineData([BinaryType.LessOrEqual, 1, 1, true])]
        internal void Binary(BinaryType type, dynamic left, dynamic right, dynamic result)
        {
            INode[] nodes = [new BinaryNode(type, new LiteralNode(left), new LiteralNode(right))];
            var expression = new Binary(type, new Literal());
            Assert.Equal(result, Interpreter.Interpret<dynamic>(nodes, expression));
        }

        [Fact]
        internal void VariableReturnsCorrectValue()
        {
            var node = new VariableNode("myVariable", new LiteralNode(1), true);
            var expression = new Variable(new Literal());
            Dictionary<string, INode> variables = [];
            Assert.Equal(new Void(), expression.Interpret(node, variables));
        }

        [Fact]
        internal void VariableAssignsNewLabel()
        {
            var node = new VariableNode("myVariable", new LiteralNode(1), true);
            var expression = new Variable(new Literal());
            Dictionary<string, INode> variables = [];
            expression.Interpret(node, variables);
            Assert.Equal(new LiteralNode(1), variables["myVariable"]);
        }
    }
}
