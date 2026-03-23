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
        [InlineData("2 + 2 < 1 * 4", false)]
        public void Precedence(string script, dynamic expected)
        {
            Assert.Equal(expected, Frostscript.Run<dynamic>(script));
        }
    }
}
