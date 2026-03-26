using Frostscript.Internal;

namespace Frostscript.Expressions
{
    internal interface IExpression
    {
        public (INode, Token[]) Parse(Token[] tokens);
        public dynamic Interpret(INode node, IDictionary<string, dynamic> variables);
        public IValidationResult Validate(NodeContext context, IDictionary<string, ValidationVariable> variables);
    }
}
