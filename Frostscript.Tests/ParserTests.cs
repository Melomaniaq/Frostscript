using Frostscript.Domain;
using Frostscript.Domain.Features;
using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Parser;
using Xunit;
using IParseResult =
    Frostscript.Domain.IResult<
        Frostscript.Domain.Parser.ParseSuccess,
        Frostscript.Domain.Parser.ParseError[]
    >;


namespace Frostscript.Tests
{
    public class ParserTests
    {
        [Fact]
        public void Literal()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, "Hello")];
            var expression = new Literal();
            var expected = new IParseResult.Pass(new ParseSuccess(new LiteralNode("Hello", tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Theory]
        [InlineData(TokenType.Plus, BinaryType.Addition)]
        [InlineData(TokenType.Minus, BinaryType.Subtraction)]
        [InlineData(TokenType.Star, BinaryType.Multiplication)]
        [InlineData(TokenType.ForwardSlash, BinaryType.Division)]
        [InlineData(TokenType.DoubleEqual, BinaryType.Equality)]
        [InlineData(TokenType.NotEqual, BinaryType.Inequality)]
        [InlineData(TokenType.GreaterThan, BinaryType.GreaterThan)]
        [InlineData(TokenType.GreaterOrEqual, BinaryType.GreaterOrEqual)]
        [InlineData(TokenType.LessThan, BinaryType.LessThan)]
        [InlineData(TokenType.LessOrEqual, BinaryType.LessOrEqual)]
        [InlineData(TokenType.And, BinaryType.And)]
        [InlineData(TokenType.Or, BinaryType.Or)]
        public void Binary(TokenType @operator, BinaryType type)
        {
            Token[] tokens = 
            [
                new Token(TokenType.Literal, 0, 0, 1), 
                new Token(@operator, 0, 2), 
                new Token(TokenType.Literal, 0, 3, 3), 
                new Token(@operator, 0, 4), 
                new Token(TokenType.Literal, 0, 5, 5)];
            var expression = new Binary(type, new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(new BinaryNode(
                type, 
                new LiteralNode(1, tokens[0]), 
                new BinaryNode(
                    type, 
                    new LiteralNode(3, tokens[2]), 
                    new LiteralNode(5, tokens[4]), 
                    tokens[3]),
                tokens[1]
            ), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Fact]
        public void BinaryPassthrough()
        {
            Token[] tokens = [new Token(TokenType.Literal, 0, 0, 1)];
            var expression = new Binary(BinaryType.Addition, new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(new LiteralNode(1, tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
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

            var expression = new VariableDecleration(new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(new VariableNode("myVariable", new LiteralNode(1, tokens[3]), mutable, tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
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

            var expression = new VariableDecleration(new Literal());
            Assert.IsType<IParseResult.Fail>(expression.Parse(tokens));
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

            var expression = new VariableDecleration(new Literal());
            Assert.IsType<IParseResult.Fail>(expression.Parse(tokens));
        }

        [Fact]
        public void Label()
        {

            Token[] tokens = [new Token(TokenType.Label, 0, 0, "hello")];
            var expression = new Label(new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(new LabelNode("hello", tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
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
            var expected = new IParseResult.Pass(new ParseSuccess(new AssignmentNode("hello", new LiteralNode(1, tokens[2]), tokens[1]), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Fact]
        public void FunctionMultipleParameters()
        {
            Token[] tokens =
            [
                new Token(TokenType.Fun, 0, 0),
                new Token(TokenType.Label, 0, 1, "parameter1"),
                new Token(TokenType.ParenthesesOpen, 0, 2),
                new Token(TokenType.Num, 0, 3),
                new Token(TokenType.ParenthesesClose, 0, 4),
                new Token(TokenType.Label, 0, 5, "parameter2"),
                new Token(TokenType.ParenthesesOpen, 0, 6),
                new Token(TokenType.Num, 0, 7),
                new Token(TokenType.ParenthesesClose, 0, 8),
                new Token(TokenType.Label, 0, 9, "parameter3"),
                new Token(TokenType.ParenthesesOpen, 0, 10),
                new Token(TokenType.Num, 0, 11),
                new Token(TokenType.ParenthesesClose, 0, 12),
                new Token(TokenType.Arrow, 0, 13),
                new Token(TokenType.Literal, 0, 14, 1)
            ];

            var expression = new Function(new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(new FunctionNode(
                [
                    new ("parameter1", new NumberType()), 
                    new ("parameter2", new NumberType()), 
                    new ("parameter3", new NumberType())
                ], 
                new LiteralNode(1, tokens[14]), 
                tokens[0]
            ), []));

            Assert.Equivalent(expected, expression.Parse(tokens));
        }

        [Fact]
        public void FunctionSingleParameters()
        {
            Token[] tokens =
            [
                new Token(TokenType.Fun, 0, 0),
                new Token(TokenType.Label, 0, 1, "parameter"),
                new Token(TokenType.ParenthesesOpen, 0, 2),
                new Token(TokenType.Num, 0, 3),
                new Token(TokenType.ParenthesesClose, 0, 4),
                new Token(TokenType.Arrow, 0, 5),
                new Token(TokenType.Literal, 0, 6, 1)
            ];

            var expression = new Function(new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(
                new FunctionNode([new ("parameter", new NumberType())], new LiteralNode(1, tokens[6]), tokens[0]), 
                []
            ));

            Assert.Equivalent(expected, expression.Parse(tokens));
        }

        [Fact]
        public void Call()
        {
            Token[] tokens =
            [
                new Token(TokenType.Label, 0, 0, "func"),
                new Token(TokenType.Literal, 0, 1, 1),
                new Token(TokenType.Literal, 0, 1, 2)
            ];

            var expression = new Call(new Label(new Literal()));
            var expected = new IParseResult.Pass(new ParseSuccess(new CallNode(
                new CallNode(
                    new LabelNode("func", tokens[0]), 
                    new LiteralNode(1, tokens[1]),
                    tokens[0]),
                new LiteralNode(2, tokens[2]),
                tokens[0]
            ), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Fact]
        public void CallWithTrailingParentheses()
        {
            Token[] tokens =
            [
                new Token(TokenType.Label, 0, 0, "func"),
                new Token(TokenType.Literal, 0, 1, 1),
                new Token(TokenType.ParenthesesClose, 0, 2)
            ];

            var expression = new Call(new Label(new Literal()));
            var expected = new IParseResult.Pass(new ParseSuccess(
                new CallNode(new LabelNode("func", tokens[0]), new LiteralNode(1, tokens[1]), tokens[0]), 
                [new Token(TokenType.ParenthesesClose, 0, 2)]));

            Assert.Equivalent(expected, expression.Parse(tokens));
        }

        [Fact]
        public void CallPassthroughSemiColon()
        {
            Token[] tokens =
            [
                new Token(TokenType.Label, 0, 0, "func"),
                new Token(TokenType.SemiColon, 0, 1)
            ];

            var expression = new Call(new Label(new Literal()));
            var expected = new IParseResult.Pass(new ParseSuccess(new LabelNode("func", tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Fact]
        public void CallPassthrougEndOfFile()
        {
            Token[] tokens =
            [
                new Token(TokenType.Label, 0, 0, "func"),
            ];

            var expression = new Call(new Label(new Literal()));
            var expected = new IParseResult.Pass(new ParseSuccess(new LabelNode("func", tokens[0]), []));

            Assert.Equal(expected, expression.Parse(tokens));
        }

        [Fact]
        public void Parentheses()
        {
            Token[] tokens =
            [
                new Token(TokenType.ParenthesesOpen, 0, 0),
                new Token(TokenType.Literal, 0, 1, 1),
                new Token(TokenType.ParenthesesClose, 0, 2),
            ];

            var expression = new Parentheses(new Literal());
            var expected = new IParseResult.Pass(new ParseSuccess(
                new ParenthesesNode(new LiteralNode(1, tokens[1]), tokens[0]),
                []
            ));

            Assert.Equal(expected, expression.Parse(tokens));
        }
    }
}
