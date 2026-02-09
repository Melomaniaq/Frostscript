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
        [InlineData("1 + 2", 3)]
        [InlineData("1 - 2", -1)]
        [InlineData("1 * 2", 2)]
        [InlineData("4 / 2", 2)]
        [InlineData("2 + 4 * 2", 10)]
        public void Binary(string script, int expected)
        {
            Assert.Equal(expected, Frostscript.Run<int>(script));
        }
    }
}
