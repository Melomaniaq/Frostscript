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
        [InlineData("=", TokenType.SingleEqual)]
        internal void Operators(string script, TokenType tokenType) => Assert.Equal([new Token(tokenType, 0, 0)], Lexer.Lex(script));
    }
}
