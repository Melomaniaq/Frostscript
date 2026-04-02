using Frostscript.Features;
using Frostscript.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Frostscript
{
    internal static class Interpreter
    {
        internal static dynamic Interpret(ITypedNode[] ast)
        {
            VariableDictionary globalVariables = [];

            return ast
                .Select(Convert)
                .Select(x => ExpressionTree.Interpret(x, globalVariables))
                .ToArray()
                .Last();
        }

        internal static T Interpret<T>(ITypedNode[] ast) => (T)Interpret(ast);

        static IExpression Convert(ITypedNode node)
        {
            return node switch
            {
                TypedBinaryNode binaryNode => new BinaryExpression(binaryNode.Type, Convert(binaryNode.Left), Convert(binaryNode.Right)),
                TypedVariableNode variableNode => new VariableExpression(variableNode.Label, Convert(variableNode.Value)),
                TypedLabelNode labelNode => new LabelExpression(labelNode.Label),
                TypedLiteralNode literalNode => new LiteralExpression(literalNode.Value),
                TypedAssignmentNode assignmentNode => new AssignmentExpression(assignmentNode.Label, Convert(assignmentNode)),
                TypedFunctionNode functionNode => new FunctionExpression([.. functionNode.Parameters.Select(x => x.Label)], Convert(functionNode.Body)),
                TypedCallNode callNode => new CallExpression(Convert(callNode.Left), Convert(callNode.Right)),
                TypedParenthesesNode parenthesesNode => new ParenthesesExpression(Convert(parenthesesNode.Body)),
                _ => throw new Exception($"Unhandled node type {node}")

            };
        }
    }
}
