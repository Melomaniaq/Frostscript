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

        [Fact]
        public void Binary()
        {
            INode[] nodes = [new BinaryNode(BinaryType.Addition, new LiteralNode(1), new LiteralNode(2))];
            var expression = new Binary([TokenType.Plus], new Literal(new Error()));
            Assert.Equal(3, Interpreter.Interpret<int>(nodes, expression));
        }
    }
}
