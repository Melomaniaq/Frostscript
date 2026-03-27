using Frostscript.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Expressions
{
    internal class Literal : IFeature
    {
        public ParserResult Parse(Token[] tokens)
        {
            if (tokens[0].Type != TokenType.Literal)
                return new (
                    new ErrorNode($"Unexpected token {tokens[0].Literal}", new VoidType()),
                    tokens[0],
                    [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

            IDataType dataType = tokens[0].Literal switch
            {
                sbyte or byte or short or ushort or int or uint or long or ulong or nint or nuint or float or double or decimal => new NumberType(),
                string or char => new StringType(),
                bool => new BoolType(),
                _ => throw new NotSupportedException()
            };

            return new (
                new LiteralNode(tokens[0].Literal, dataType),
                tokens[0],
                [.. tokens.Skip(1)]);
        }
        public dynamic Interpret(IExpression node, IDictionary<string, dynamic> variables)
        {
            if (node is LiteralNode literal) return literal.Value;
            if (node is ErrorNode error) throw new Exception($"Unhandled Parsing Error: {error.Error}");
            else throw new NotImplementedException("Node Could not be resolved. Did you forget to add the expression to the expression tree?");
        }

        public IValidationResult Validate(NodeContext context, IDictionary<string, VariableData> variables)
        {
            if (context.Node is LiteralNode literal) return new Pass(literal);
            else throw new NotImplementedException();
        }
    }
}
