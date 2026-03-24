using Frostscript.Expressions;
using Frostscript.Nodes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Frostscript.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void LiteralString()
        {
            Assert.Equal("Hello", Frostscript.Run<string>(@"""Hello"""));
        }

        [Theory]
        [InlineData("2 + 4 * 2", 10)]
        [InlineData("2 + 2 == 1 * 4", true)]
        [InlineData("2 + 2 != 1 * 4", false)]
        [InlineData("2 + 2 >= 1 * 4", true)]
        [InlineData("2 + 2 > 1 * 4", false)]
        [InlineData("2 + 2 <= 1 * 4", true)]
        [InlineData("true or true and false", true)]
        public void Precedence(string script, dynamic expected)
        {
            Assert.Equal(expected, Frostscript.Run<dynamic>(script));
        }

        [Fact]
        public void Variables()
        {
            Assert.Equal(2, Frostscript.Run<int>(@"let x = 2 x"));
        }

        [Fact]
        public void Assignment()
        {
            Assert.Equal(2, Frostscript.Run<int>(@"var x = 1 x = 2 x"));
        }
    }
}
