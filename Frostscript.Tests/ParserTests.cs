using Frostscript.Expressions;
using Frostscript.Nodes;
using System.Linq.Expressions;
using Xunit;

namespace Frostscript.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Literal()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, "Hello")];
            var expression = new Literal(new Error());
            INode[] expected = [new LiteralNode("Hello")];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
        }

        [Theory]
        [InlineData(TokenType.Plus, BinaryType.Addition)]
        [InlineData(TokenType.Minus, BinaryType.Subtraction)]
        [InlineData(TokenType.Star, BinaryType.Multiplication)]
        [InlineData(TokenType.ForwardSlash, BinaryType.Devision)]
        internal void BinaryAddition(TokenType @operator, BinaryType type)
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1), new Token(@operator, 0, 2), new Token(TokenType.Literal, 0, 4, 2)];
            var expression = new Binary([@operator], new Literal(new Error()));
            INode[] expected = [new BinaryNode(type, new LiteralNode(1), new LiteralNode(2))];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
        }

        [Fact]
        public void BinaryPassthrough()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1)];
            var expression = new Binary([TokenType.Plus], new Literal(new Error()));
            INode[] expected = [new LiteralNode(1)];

            Assert.Equal(expected, Parser.Parse(tokens, expression));
        }
    }
}
