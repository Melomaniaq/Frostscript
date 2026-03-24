using Frostscript.Expressions;
using Frostscript.Nodes;
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
            INode[] expected = [new LiteralNode("Hello")];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
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
        internal void BinaryAddition(TokenType @operator, BinaryType type)
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1), new Token(@operator, 0, 2), new Token(TokenType.Literal, 0, 4, 2)];
            var expression = new Binary(type, new Literal());
            INode[] expected = [new BinaryNode(type, new LiteralNode(1), new LiteralNode(2))];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
        }

        [Fact]
        public void BinaryPassthrough()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1)];
            var expression = new Binary(BinaryType.Addition, new Literal());
            INode[] expected = [new LiteralNode(1)];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
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
            INode[] expected = [new VariableNode("myVariable", new LiteralNode(1), mutable)];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
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
            Assert.IsType<ErrorNode>(Parser.Parse(tokens, expression).First());
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
            Assert.IsType<ErrorNode>(Parser.Parse(tokens, expression).First());
        }
    }
}
