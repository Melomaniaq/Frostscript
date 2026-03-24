using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Frostscript.Tests
{
    public class LexerTests
    {
        [Fact]
        public void LiteralString()
        {
            Token[] expected = [new Token(TokenType.Literal, 0, 0, "Hello")];
            var actual = Lexer.Lex(@"""Hello""");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("1223")]
        [InlineData("1.35")]
        public void LiteralNumber(string script) => Assert.Equal([new Token(TokenType.Literal, 0, 0, decimal.Parse(script))], Lexer.Lex(script));
        
        [Fact]
        public void Label() => Assert.Equal([new Token(TokenType.Label, 0, 0, "Hello")], Lexer.Lex(@"Hello"));

        [Theory]
        [InlineData("+", TokenType.Plus)]
        [InlineData("-", TokenType.Minus)]
        [InlineData("/", TokenType.ForwardSlash)]
        [InlineData("*", TokenType.Star)]
        [InlineData("!", TokenType.Not)]
        [InlineData("!=", TokenType.NotEqual)]
        [InlineData("==", TokenType.DoubleEqual)]
        [InlineData(">", TokenType.GreaterThan)]
        [InlineData(">=", TokenType.GreaterOrEqual)]
        [InlineData("<", TokenType.LessThan)]
        [InlineData("<=", TokenType.LessOrEqual)]
        internal void Operators(string script, TokenType tokenType) => Assert.Equal([new Token(tokenType, 0, 0)], Lexer.Lex(script));

        [Fact]
        public void Var() => Assert.Equal([new Token(TokenType.Var, 0, 0)], Lexer.Lex("var"));

        [Fact]
        public void Let() => Assert.Equal([new Token(TokenType.Let, 0, 0)], Lexer.Lex("let"));

        [Fact]
        public void And() => Assert.Equal([new Token(TokenType.And, 0, 0)], Lexer.Lex("and"));

        [Fact]
        public void Or() => Assert.Equal([new Token(TokenType.Or, 0, 0)], Lexer.Lex("or"));

        [Fact]
        public void True() => Assert.Equal([new Token(TokenType.Literal, 0, 0, true)], Lexer.Lex("true"));

        [Fact]
        public void False() => Assert.Equal([new Token(TokenType.Literal, 0, 0, false)], Lexer.Lex("false"));
    }
}
