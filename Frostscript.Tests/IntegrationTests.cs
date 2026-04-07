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
        [InlineData("5 - 3 + 4 * 3", 14)]
        [InlineData("2 - 4 + 2", 0)]
        [InlineData("2 + 2 == 1 * 4", true)]
        [InlineData("2 + 2 != 1 * 4", false)]
        [InlineData("2 + 2 >= 1 * 4", true)]
        [InlineData("2 + 2 > 1 * 4", false)]
        [InlineData("2 + 2 <= 1 * 4", true)]
        [InlineData("(fun x(num) -> x) 2", 2)]
        [InlineData("true or true and false", true)]
        public void Precedence(string script, dynamic expected)
        {
            Assert.Equal(expected, Frostscript.Run<dynamic>(script));
        }

        [Fact]
        public void Variables()
        {
            Assert.Equal(2, Frostscript.Run<int>(@"let x = 2; x"));
        }

        [Fact]
        public void Assignment()
        {
            Assert.Equal(2, Frostscript.Run<int>(@"var x = 1; x = 2; x"));
        }

        [Fact]
        public void Functions()
        {
            Assert.Equal(2, Frostscript.Run<int>(@"let test = fun x(num) -> x; test 2"));
        }

        [Fact]
        public void Closure()
        {
            Assert.Equal(5, Frostscript.Run<int>(
            @"
                let x = 2; 
                let add = fun y(num) -> 
                    x + y; 
                add 3
            "));
            Assert.Equal(5, Frostscript.Run<int>(
            @"
                var x = 2; 
                let incrementX = fun y(num) -> 
                    x = x + y; 
                incrementX 3; 
                x
            "));
        }
    }
}
