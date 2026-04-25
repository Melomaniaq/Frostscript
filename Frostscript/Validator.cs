using Frostscript.Domain;
using Frostscript.Domain.Features;
using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Validator;
using MalFunction.Result;

namespace Frostscript
{
    internal static class Validator
    {
        internal static IResult<IExpression[], ValidationError[]> Validate(INode[] ast)
        {
            Dictionary<string, VariableData> globalVariables = [];

            return ast
                .Traverse(node => ExpressionTree.Validate(node, globalVariables))
                .Map(x => x.Select(Convert).ToArray());
                
        }

        static IExpression Convert(ITypedNode node)
        {
            return node switch
            {
                TypedBinaryNode binaryNode => new BinaryExpression(binaryNode.Type, Convert(binaryNode.Left), Convert(binaryNode.Right)),
                TypedVariableNode variableNode => new VariableExpression(variableNode.Label, Convert(variableNode.Value)),
                TypedLabelNode labelNode => new LabelExpression(labelNode.Label),
                TypedLiteralNode literalNode => new LiteralExpression(literalNode.Value),
                TypedAssignmentNode assignmentNode => new AssignmentExpression(assignmentNode.Label, Convert(assignmentNode.Value)),
                TypedFunctionNode functionNode => new FunctionExpression([.. functionNode.Parameters.Select(x => x.Label)], Convert(functionNode.Body)),
                TypedCallNode callNode => new CallExpression(Convert(callNode.Left), Convert(callNode.Right)),
                TypedParenthesesNode parenthesesNode => new ParenthesesExpression(Convert(parenthesesNode.Body)),
                _ => throw new Exception($"Unhandled node type {node}")
            };
        }
    }
}
