using Frostscript.Domain.Internal;

namespace Frostscript.Domain.Features
{
    public static class ExpressionTree
    {
        private static readonly IFeature expressionTree =
            new VariableDecleration(
            new Assignment(
            new Function(
            new Call(
            new Binary(BinaryType.Or,
            new Binary(BinaryType.And,
            new Binary(BinaryType.Inequality,
            new Binary(BinaryType.Equality,
            new Binary(BinaryType.GreaterThan,
            new Binary(BinaryType.GreaterOrEqual,
            new Binary(BinaryType.LessThan,
            new Binary(BinaryType.LessOrEqual,
            new Binary(BinaryType.Addition,
            new Binary(BinaryType.Subtraction,
            new Binary(BinaryType.Division,
            new Binary(BinaryType.Multiplication,
            new Parentheses(
            new Label(
            new Literal()))))))))))))))))));

        public static IValidationResult Validate(INode node, IDictionary<string, VariableData> variables) => expressionTree.Validate(node, variables);
        public static (INode, Token[]) Parse(Token[] tokens) => expressionTree.Parse(tokens);
        public static dynamic Interpret(IExpression expression, IDictionary<string, dynamic> variables) => expressionTree.Interpret(expression, variables);
    }
}
