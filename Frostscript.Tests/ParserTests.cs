using Frostscript.Expressions;
using Xunit;

namespace Frostscript.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Literal()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, "Hello")];
            var expression = new Literal();
            INode expected = new LiteralNode("Hello");

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Theory]
        [InlineData(TokenType.Plus, BinaryType.Addition)]
        [InlineData(TokenType.Minus, BinaryType.Subtraction)]
        [InlineData(TokenType.Star, BinaryType.Multiplication)]
        [InlineData(TokenType.ForwardSlash, BinaryType.Devision)]
        [InlineData(TokenType.DoubleEqual, BinaryType.Equality)]
        [InlineData(TokenType.NotEqual, BinaryType.Inequality)]
        [InlineData(TokenType.GreaterThan, BinaryType.GreaterThan)]
        [InlineData(TokenType.GreaterOrEqual, BinaryType.GreaterOrEqual)]
        [InlineData(TokenType.LessThan, BinaryType.LessThan)]
        [InlineData(TokenType.LessOrEqual, BinaryType.LessOrEqual)]
        [InlineData(TokenType.And, BinaryType.And)]
        [InlineData(TokenType.Or, BinaryType.Or)]
        internal void Binary(TokenType @operator, BinaryType type)
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1), new Token(@operator, 0, 2), new Token(TokenType.Literal, 0, 4, 2)];
            var expression = new Binary(type, new Literal());
            var expected = new BinaryNode(type, new LiteralNode(1), new LiteralNode(2));

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Fact]
        public void BinaryPassthrough()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1)];
            var expression = new Binary(BinaryType.Addition, new Literal());
            var expected = new LiteralNode(1);

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Variable(bool mutable)
        {
            Token[] tokens = 
            [
                new Token(mutable ? TokenType.Var : TokenType.Let, 0, 0),
                new Token(TokenType.Label, 0, 1, "myVariable"),
                new Token(TokenType.SingleEqual, 0, 2),
                new Token(TokenType.Literal, 0, 3, 1)
            ];

            var expression = new Variable(new Literal());
            var expected = new VariableNode("myVariable", new LiteralNode(1), mutable);

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void VariableNoEqual(bool mutable)
        {
            Token[] tokens =
            [
                new Token(mutable ? TokenType.Var : TokenType.Let, 0, 0),
                new Token(TokenType.Label, 0, 1, "myVariable"),
                new Token(TokenType.Literal, 0, 3, 1)
            ];

            var expression = new Variable(new Literal());
            Assert.IsType<ErrorNode>(expression.Parse(tokens).Item1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void VariableNoLabel(bool mutable)
        {
            Token[] tokens =
            [
                new Token(mutable ? TokenType.Var : TokenType.Let, 0, 0),
                new Token(TokenType.SingleEqual, 0, 2),
                new Token(TokenType.Literal, 0, 3, 1)
            ];

            var expression = new Variable(new Literal());
            Assert.IsType<ErrorNode>(expression.Parse(tokens).Item1);
        }

        [Fact]
        public void Label()
        {

            Token[] tokens = [new Token(TokenType.Label, 0, 0, "hello")];
            var expression = new Label(new Literal());
            var expected = new LabelNode("hello");

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Fact]
        public void Assignment()
        {
            Token[] tokens = 
            [
                new Token(TokenType.Label, 0, 0, "hello"), 
                new Token(TokenType.SingleEqual, 0, 1), 
                new Token(TokenType.Literal, 0, 2, 1)
            ];

            var expression = new Assignment(new Literal());
            var expected = new AssignmentNode("hello", new LiteralNode(1));

            Assert.Equal((expected, []), expression.Parse(tokens));
        }

        [Fact]
        public void FunctionMultipleParameters()
        {
            Token[] tokens =
            [
                new Token(TokenType.Fun, 0, 0),
                new Token(TokenType.Label, 0, 1, "parameter1"),
                new Token(TokenType.Label, 0, 1, "parameter2"),
                new Token(TokenType.Label, 0, 1, "parameter3"),
                new Token(TokenType.Arrow, 0, 2),
                new Token(TokenType.Literal, 0, 3, 1)
            ];

            var expression = new Function(new Literal());
            var expected = new FunctionNode(["parameter1", "parameter2", "parameter3"], new LiteralNode(1));
            var (actual, remainingTokens) = expression.Parse(tokens);

            Assert.Empty(remainingTokens);
            Assert.Equivalent(expected, actual);
        }

        [Fact]
        public void FunctionSingleParameters()
        {
            Token[] tokens =
            [
                new Token(TokenType.Fun, 0, 0),
                new Token(TokenType.Label, 0, 1, "parameter"),
                new Token(TokenType.Arrow, 0, 2),
                new Token(TokenType.Literal, 0, 3, 1)
            ];

            var expression = new Function(new Literal());
            var expected = new FunctionNode(["parameter"], new LiteralNode(1));
            var (actual, remainingTokens) = expression.Parse(tokens);

            Assert.Empty(remainingTokens);
            Assert.Equivalent(expected, actual);
        }
    }
}
