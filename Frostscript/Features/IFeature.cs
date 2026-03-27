using Frostscript.Internal;

namespace Frostscript.Expressions
{
    internal interface IFeature
    {
        public dynamic Interpret(IExpression node, IDictionary<string, dynamic> variables);
        public (INode, Token[]) Parse(Token[] tokens);
        public IValidationResult Validate(INode node, IDictionary<string, VariableData> variables);
    }
}
