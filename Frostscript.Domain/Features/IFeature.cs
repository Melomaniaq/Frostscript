using Frostscript.Domain.Internal;

namespace Frostscript.Domain.Features
{
    public interface IFeature
    {
        public dynamic Interpret(IExpression expression, IDictionary<string, object> variables);
        public (INode, Token[]) Parse(Token[] tokens);
        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables);
    }
}
