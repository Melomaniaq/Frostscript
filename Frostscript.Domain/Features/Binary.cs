using Frostscript.Domain.Internal;

namespace Frostscript.Domain.Features
{
    public class Binary(BinaryType type, IFeature next) : IFeature
    {
        readonly Dictionary<BinaryType, TokenType> operatorMap = new() 
        { 
            { BinaryType.Addition, TokenType.Plus }, 
            { BinaryType.Subtraction, TokenType.Minus }, 
            { BinaryType.Multiplication, TokenType.Star }, 
            { BinaryType.Division, TokenType.ForwardSlash }, 
            { BinaryType.Equality, TokenType.DoubleEqual }, 
            { BinaryType.Inequality, TokenType.NotEqual }, 
            { BinaryType.GreaterThan, TokenType.GreaterThan }, 
            { BinaryType.GreaterOrEqual, TokenType.GreaterOrEqual }, 
            { BinaryType.LessThan, TokenType.LessThan }, 
            { BinaryType.LessOrEqual, TokenType.LessOrEqual },
            { BinaryType.And, TokenType.And },
            { BinaryType.Or, TokenType.Or },
        };
      
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is BinaryExpression binary)
            {
                var left = next.Interpret(binary.Left, variables);
                var right = next.Interpret(binary.Right, variables);

                return binary.Type switch 
                { 
                    BinaryType.Addition => left + right,
                    BinaryType.Subtraction => left - right,
                    BinaryType.Multiplication => left * right,
                    BinaryType.Division => left / right,
                    BinaryType.Equality => left == right,
                    BinaryType.Inequality => left != right,
                    BinaryType.GreaterThan => left > right,
                    BinaryType.GreaterOrEqual => left >= right,
                    BinaryType.LessThan => left < right,
                    BinaryType.LessOrEqual => left <= right,
                    BinaryType.And => left && right,
                    BinaryType.Or => left || right,
                };
            }
            else return next.Interpret(expression, variables);
        }
        public (INode, Token[]) Parse(Token[] tokens)
        {
            var (left, tokensAfterLeft) = next.Parse(tokens);
            if (tokensAfterLeft.Length == 0) return (left, tokensAfterLeft);

            if (operatorMap[type] == tokensAfterLeft[0].Type)
            {
                var (right, tokensAfterRight) = Parse([.. tokensAfterLeft.Skip(1)]);
                return (new BinaryNode(type, left, right, tokensAfterLeft[0]), tokensAfterRight);
            }
            else return (left, tokensAfterLeft);
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is BinaryNode binary)
            {
                return next.Validate(binary.Left, variables)
                    .Bind(left => next.Validate(binary.Right, variables)
                        .Bind(right =>
                        {
                            TypedBinaryNode BinaryOFType(IDataType dataType) => new (binary.Type, left, right, dataType);

                            return binary.Type switch
                            {
                                BinaryType.Subtraction or
                                BinaryType.Multiplication or
                                BinaryType.Division or
                                BinaryType.LessThan or
                                BinaryType.LessOrEqual or
                                BinaryType.GreaterOrEqual or
                                BinaryType.GreaterThan => (left.DataType, right.DataType) switch
                                {
                                    (NumberType, NumberType) => new IValidationResult.Pass(BinaryOFType(new NumberType())) as IValidationResult,
                                    _ => new IValidationResult.Fail((
                                        binary.Token,
                                        $"Operator {binary.Type} cannot be use with types {left.DataType} and {right.DataType}"
                                    )),
                                },
                                BinaryType.Addition => (left.DataType, right.DataType) switch
                                {
                                    (NumberType, NumberType) => new IValidationResult.Pass(BinaryOFType(new NumberType())),
                                    (StringType, StringType) => new IValidationResult.Pass(BinaryOFType(new StringType())),
                                    _ => new IValidationResult.Fail((
                                        binary.Token,
                                       $"type {left.DataType} cannot be additioned with type {right.DataType}"
                                    )),
                                },
                                BinaryType.Equality or BinaryType.Inequality => new IValidationResult.Pass(new TypedBinaryNode(binary.Type, left, right, new BoolType())),
                                BinaryType.And or BinaryType.Or => (left.DataType, right.DataType) switch
                                {
                                    (BoolType, BoolType) => new IValidationResult.Pass(BinaryOFType(new BoolType())),
                                    _ => new IValidationResult.Fail((
                                        binary.Token,
                                        $"Both sides of {binary.Type} must be a Bool. {left.DataType} and {right.DataType} where given"
                                    )),
                                },
                                _ => throw new NotImplementedException()
                            };
                        })
                    );
            }
            else return next.Validate(node, variables);
        }
    }
}
