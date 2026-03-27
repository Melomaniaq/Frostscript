using Frostscript.Features;
using Frostscript.Internal;
using Frostscript.Types;
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
            var node = new LiteralNode("Hello");
            var expression = new Literal();
            Assert.Equal("Hello", expression.Interpret(node, new VariableDictionary()));
        }

        [Theory]
        [InlineData([BinaryType.Addition, 1, 2, 3])]
        [InlineData([BinaryType.Subtraction, 1, 2, -1])]
        [InlineData([BinaryType.Multiplication, 2, 3, 6])]
        [InlineData([BinaryType.Division, 6, 3, 2])]

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

        [InlineData([BinaryType.And, true, true, true])]
        [InlineData([BinaryType.And, true, false, false])]
        [InlineData([BinaryType.And, false, false, false])]

        [InlineData([BinaryType.Or, true, false, true])]
        [InlineData([BinaryType.Or, false, true, true])]
        [InlineData([BinaryType.Or, true, true, true])]
        [InlineData([BinaryType.Or, false, false, false])]
        internal void Binary(BinaryType type, dynamic left, dynamic right, dynamic result)
        {
            var node = new BinaryNode(type, new LiteralNode(left), new LiteralNode(right));
            var expression = new Binary(type, new Literal());
            Assert.Equal(result, expression.Interpret(node, new VariableDictionary()));
        }

        [Fact]
        internal void VariableReturnsCorrectValue()
        {
            var node = new VariableNode("myVariable", new LiteralNode(1), true);
            var expression = new Variable(new Literal());
            Assert.Equal(new FSVoid(), expression.Interpret(node, new VariableDictionary()));
        }

        [Fact]
        internal void VariableAssignsNewLabel()
        {
            var node = new VariableNode("myVariable", new LiteralNode(1), true);
            var expression = new Variable(new Literal());
            var variables = new VariableDictionary();
            expression.Interpret(node, variables);
            Assert.Equal(1, variables["myVariable"]);
        }

        [Fact]
        internal void Label()
        {
            var node = new LabelNode("hello");
            var variables = new VariableDictionary { { "hello", 1 } };
            var expression = new Label(new Literal());
            Assert.Equal(1, expression.Interpret(node, variables));
        }

        [Fact]
        internal void Assignment()
        {
            var node = new AssignmentNode("hello", new LiteralNode(2));
            var variables = new VariableDictionary { { "hello", new LiteralNode(1) } };
            var expression = new Assignment(new Literal());
            Assert.Equal(new FSVoid(), expression.Interpret(node, variables));
            Assert.Equal(2, variables["hello"]);
        }

        [Fact]
        internal void Function()
        {
            var node = new FunctionNode(["parameter1", "parameter2"], new LiteralNode(1));
            var variables = new VariableDictionary { { "variable", new LiteralNode(1) } };
            var expression = new Function(new Literal());

            var firstClosure = new Closure(variables);

            var expected = new FSFunction(
                "parameter1",
                new LiteralNode(new FSFunction(
                    "parameter2",
                    new LiteralNode(1),
                    new Closure(firstClosure)
                )),
                firstClosure
            );

            Assert.Equivalent(expected, expression.Interpret(node, variables)
            );
        }

        [Fact]
        internal void Call()
        {
            var variables = new VariableDictionary();
            var node = new CallNode(
                new LiteralNode(
                    new FSFunction(
                        "parameter", 
                        new LabelNode("parameter"), 
                        new VariableDictionary()
                    )
                ), 
                new LiteralNode(1)
            );
            var expression = new Call(new Label(new Literal()));

            Assert.Equal(1, expression.Interpret(node, variables));
        }

        [Fact]
        internal void Parentheses()
        {
            var node = new ParenthesesNode(new LiteralNode(1));
            var expression = new Parentheses(new Literal());
            Assert.Equal(1, expression.Interpret(node, new VariableDictionary()));
        }
    }
}
