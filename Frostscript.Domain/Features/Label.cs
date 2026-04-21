using Frostscript.Domain.Internal;

namespace Frostscript.Domain.Features
{
    public class Label(IFeature Next) : IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables)
        {
            if (expression is LabelExpression label) 
                return variables[label.Label];
            else 
                return Next.Interpret(expression, variables);
        }

        public IParseResult Parse(Token[] tokens)
        {
            if (tokens[0].Type is TokenType.Label) 
                return new IParseResult.Pass(new (new LabelNode(tokens[0].Literal, tokens[0]), [.. tokens.Skip(1)]));
            else 
                return Next.Parse(tokens);
        }

        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables)
        {
            if (node is LabelNode label)
            {
                if (variables.TryGetValue(label.Label, out var variable))
                    return new IValidationResult.Pass(new TypedLabelNode(label.Label, variable.DataType));
                else 
                    return new IValidationResult.Fail(new (label.Token, $"Label '{label.Label}' does not exist within scope"));
            }
            else return Next.Validate(node, variables);
        }
    }
}
