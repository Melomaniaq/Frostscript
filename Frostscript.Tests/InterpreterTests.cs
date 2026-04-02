using Frostscript.Features;
using Frostscript.Internal;
using Frostscript.Types;
using Xunit;

namespace Frostscript.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void LiteralString()
        {
            var Expression = new LiteralExpression("Hello");
            var expression = new Literal();
            Assert.Equal("Hello", expression.Interpret(Expression, new VariableDictionary()));
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
            var Expression = new BinaryExpression(type, new LiteralExpression(left), new LiteralExpression(right));
            var expression = new Binary(type, new Literal());
            Assert.Equal(result, expression.Interpret(Expression, new VariableDictionary()));
        }

        [Fact]
        internal void VariableReturnsCorrectValue()
        {
            var Expression = new VariableExpression("myVariable", new LiteralExpression(1));
            var expression = new VariableDecleration(new Literal());
            Assert.Equal(new FSVoid(), expression.Interpret(Expression, new VariableDictionary()));
        }

        [Fact]
        internal void VariableAssignsNewLabel()
        {
            var Expression = new VariableExpression("myVariable", new LiteralExpression(1));
            var expression = new VariableDecleration(new Literal());
            var variables = new VariableDictionary();
            expression.Interpret(Expression, variables);
            Assert.Equal(1, variables["myVariable"]);
        }

        [Fact]
        internal void Label()
        {
            var Expression = new LabelExpression("hello");
            var variables = new VariableDictionary { { "hello", 1 } };
            var expression = new Label(new Literal());
            Assert.Equal(1, expression.Interpret(Expression, variables));
        }

        [Fact]
        internal void Assignment()
        {
            var Expression = new AssignmentExpression("hello", new LiteralExpression(2));
            var variables = new VariableDictionary { { "hello", new LiteralExpression(1) } };
            var expression = new Assignment(new Literal());
            Assert.Equal(new FSVoid(), expression.Interpret(Expression, variables));
            Assert.Equal(2, variables["hello"]);
        }

        [Fact]
        internal void Function()
        {
            var Expression = new FunctionExpression(["parameter1", "parameter2"], new LiteralExpression(1));
            var variables = new VariableDictionary { { "variable", new LiteralExpression(1) } };
            var expression = new Function(new Literal());

            var firstClosure = new Closure<string, object>(variables);

            var expected = new FSFunction(
                "parameter1",
                new LiteralExpression(new FSFunction(
                    "parameter2",
                    new LiteralExpression(1),
                    new Closure<string, object>(firstClosure)
                )),
                firstClosure
            );

            Assert.Equivalent(expected, expression.Interpret(Expression, variables)
            );
        }

        [Fact]
        internal void Call()
        {
            var variables = new VariableDictionary();
            var Expression = new CallExpression(
                new LiteralExpression(
                    new FSFunction(
                        "parameter", 
                        new LabelExpression("parameter"), 
                        new VariableDictionary()
                    )
                ), 
                new LiteralExpression(1)
            );
            var expression = new Call(new Label(new Literal()));

            Assert.Equal(1, expression.Interpret(Expression, variables));
        }

        [Fact]
        internal void Parentheses()
        {
            var Expression = new ParenthesesExpression(new LiteralExpression(1));
            var expression = new Parentheses(new Literal());
            Assert.Equal(1, expression.Interpret(Expression, new VariableDictionary()));
        }
    }
}
