using Frostscript.Domain.Internal;
using Frostscript.Domain.Types;

namespace Frostscript.Domain.Features
{
    public class VariableDecleration(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is VariableExpression variable)
            {
                variables[variable.Label] = Next.Interpret(variable.Value, variables);
                return new FSVoid();
            }
            else return Next.Interpret(expression, variables);
        }
        public (INode, Token[]) Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Let or TokenType.Var)
            {
                if (tokens[1].Type is not TokenType.Label)
                    return (
                        new ErrorNode($"Expected Label", tokens[1]),
                        [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]
                    );
              
                if (tokens[2].Type is not TokenType.SingleEqual)
                    return (new ErrorNode($"Expected '='", tokens[2]), [.. tokens.SkipWhile(x => x.Type is not TokenType.SemiColon)]);

                var (value, valueTokens) = Next.Parse([.. tokens.Skip(3)]);
                return (new VariableNode(tokens[1].Literal, value, tokens[0].Type is TokenType.Var, tokens[0]), valueTokens);
            }
            else return Next.Parse(tokens);
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is VariableNode variable)
            {
                return Next.Validate(variable.Value, variables)
                    .Bind(value =>
                    {
                        variables[variable.Label] = new VariableData(value.DataType, variable.Mutable);
                        return new IValidationResult.Pass(new TypedVariableNode(variable.Label, value, variable.Mutable, new VoidType()));
                    });
            }
            else return Next.Validate(node, variables);
        }
    }
}
