
using Frostscript.Domain.Features;
using Frostscript.Domain.Internal;
using Xunit;
using IValidationResult =
    Frostscript.Domain.Internal.IResult<
        Frostscript.Domain.Internal.ITypedNode,
        (Frostscript.Domain.Internal.Token token, string error)
    >;

namespace Frostscript.Tests
{
    public class ValidatorTests
    {

        [Theory]
        [InlineData((sbyte)1)]
        [InlineData((byte)1)]
        [InlineData((short)1)]
        [InlineData((ushort)1)]
        [InlineData((int)1)]
        [InlineData((uint)1)]
        [InlineData((long)1)]
        [InlineData((ulong)1)]
        [InlineData((float)1)]
        [InlineData((double)1)]
        [InlineData('1')]
        [InlineData(true)]
        public void Literal(dynamic value)
        {
            var node = new LiteralNode(value, new Token(TokenType.Literal, 0, 0, 1));
            var expression = new Literal();
            IDataType dataType = value switch
            {
                sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or float or double or decimal => new NumberType(),
                string or char => new StringType(),
                bool => new BoolType(),
                _ => throw new NotSupportedException()
            };

            var expected = new IValidationResult.Pass(new TypedLiteralNode(value, dataType));

            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }

        [Theory]
        [InlineData([BinaryType.Addition, 1, 2])]
        [InlineData([BinaryType.Subtraction, 1, 2])]
        [InlineData([BinaryType.Multiplication, 2, 3])]
        [InlineData([BinaryType.Division, 6, 3])]

        public void BinaryMathimaticalOperators(BinaryType binaryType, dynamic left, dynamic right)
        {
            var node = new BinaryNode(
                binaryType,
                new LiteralNode(left, new Token()),
                new LiteralNode(right, new Token()),
                new Token()
            );
            var expression = new Binary(binaryType, new Literal());

            var expected = new IValidationResult.Pass(
                new TypedBinaryNode(
                    binaryType,
                    new TypedLiteralNode(left, new NumberType()),
                    new TypedLiteralNode(right, new NumberType()),
                    new NumberType()
                    )
                );

            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }

        [Theory]
        [InlineData([BinaryType.Equality, 1, 1])]
        [InlineData([BinaryType.Equality, 1, 2])]

        [InlineData([BinaryType.Inequality, 1, 1])]
        [InlineData([BinaryType.Inequality, 1, 2])]     
        [InlineData([BinaryType.GreaterThan, 1, 2])]
        [InlineData([BinaryType.GreaterThan, 2, 1])]
        [InlineData([BinaryType.GreaterThan, 1, 1])]
        [InlineData([BinaryType.GreaterOrEqual, 1, 2])]
        [InlineData([BinaryType.GreaterOrEqual, 2, 1])]
        [InlineData([BinaryType.GreaterOrEqual, 1, 1])]

        [InlineData([BinaryType.LessThan, 1, 2])]
        [InlineData([BinaryType.LessThan, 2, 1])]
        [InlineData([BinaryType.LessThan, 1, 1])]
        [InlineData([BinaryType.LessOrEqual, 1, 2])]
        [InlineData([BinaryType.LessOrEqual, 2, 1])]
        [InlineData([BinaryType.LessOrEqual, 1, 1])]
        public void BinaryComparisonOperators(BinaryType binaryType, dynamic left, dynamic right)
        {
            var node = new BinaryNode(
                binaryType,
                new LiteralNode(left, new Token()),
                new LiteralNode(right, new Token()),
                new ()
            );
            var expression = new Binary(binaryType, new Literal());

            var expected = new IValidationResult.Pass(
                new TypedBinaryNode(
                    binaryType,
                    new TypedLiteralNode(left, new NumberType()),
                    new TypedLiteralNode(right, new NumberType()),
                    new BoolType()
                    )
                );

            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }

        [Theory]
        [InlineData([BinaryType.And, true, true])]
        [InlineData([BinaryType.And, true, false])]
        [InlineData([BinaryType.And, false, false])]

        [InlineData([BinaryType.Or, true, false])]
        [InlineData([BinaryType.Or, false, true])]
        [InlineData([BinaryType.Or, true, true])]
        [InlineData([BinaryType.Or, false, false])]
        public void BinaryLogicalOperators(BinaryType binaryType, dynamic left, dynamic right)
        {
            var node = new BinaryNode(
                binaryType,
                new LiteralNode(left, new Token()),
                new LiteralNode(right, new Token()),
                new ()
            );
            var expression = new Binary(binaryType, new Literal());

            var expected = new IValidationResult.Pass(
                new TypedBinaryNode(
                    binaryType,
                    new TypedLiteralNode(left, new BoolType()),
                    new TypedLiteralNode(right, new BoolType()),
                    new BoolType()
                    )
                );

            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }

        [Fact]
        public void Variable()
        {
            var node = new VariableNode("myVariable", new LiteralNode(1, new ()), true, new ());
            var expression = new VariableDecleration(new Literal());

            var expected = new IValidationResult.Pass(new TypedVariableNode("myVariable", new TypedLiteralNode(1, new NumberType()), true, new VoidType()));
            
            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }

        [Fact]
        public void Label()
        {
            var node = new LabelNode("hello", new());
            var variables = new Dictionary<string, VariableData> { { "hello", new(new NumberType(), true) } };
            var expression = new Label(new Literal());

            var expected = new IValidationResult.Pass(new TypedLabelNode("hello", new NumberType()));

            Assert.Equal(expected, expression.Validate(node, variables));
        }

        [Fact]
        public void Assignment()
        {
            var node = new AssignmentNode("hello", new LiteralNode(2, new ()), new());
            var variables = new Dictionary<string, VariableData> { { "hello", new(new NumberType(), true) } };
            var expression = new Assignment(new Literal());

            var expected = new IValidationResult.Pass(
                new TypedAssignmentNode(
                    "hello", 
                    new TypedLiteralNode(2, new NumberType()), 
                    new VoidType()
                )
            );

            Assert.Equal(expected, expression.Validate(node, variables));
        }

        [Fact]
        public void Function()
        {
            var node = new FunctionNode(
                [("parameter1", new NumberType()), ("parameter2", new NumberType())], 
                new LiteralNode(1, new()), 
                new()
            );

            var variables = new Dictionary<string, VariableData> { { "variable", new(new NumberType(), false) } };
            var expression = new Function(new Literal());

            var expected = new IValidationResult.Pass(
                new TypedFunctionNode(
                    [("parameter1", new NumberType()), ("parameter2", new NumberType())],
                    new TypedLiteralNode(1, new NumberType()),
                    new FunctionType(
                        new NumberType(),
                        new FunctionType(new NumberType(), new NumberType())
                    )
                )
            );

            Assert.Equivalent(expected, expression.Validate(node, variables));
        }

        [Fact]
        public void Call()
        {
            var variables = new Dictionary<string, VariableData>();
            var node = new CallNode(
                new FunctionNode(
                    [("parameter1", new NumberType())],
                    new LiteralNode(1, new()), 
                    new()
                ),
                new LiteralNode(1, new()),
                new()
            );
            var expression = new Call(new Function(new Literal()));
            var expected = new IValidationResult.Pass(
                new TypedCallNode(
                    new TypedFunctionNode(
                        [("parameter1", new NumberType())],
                        new TypedLiteralNode(1, new NumberType()),
                        new FunctionType(new NumberType(), new NumberType())
                    ),
                    new TypedLiteralNode(1, new NumberType()),
                new NumberType()
            ));

            Assert.Equivalent(expected, expression.Validate(node, variables));
        }

        [Fact]
        public void Parentheses()
        {
            var node = new ParenthesesNode(new LiteralNode(1, new()), new());
            var expression = new Parentheses(new Literal());
            var expected = new IValidationResult.Pass(
                new TypedParenthesesNode(
                    new TypedLiteralNode(1, new NumberType()), 
                    new NumberType()
                )
            );

            Assert.Equal(expected, expression.Validate(node, new Dictionary<string, VariableData>()));
        }
    }
}
