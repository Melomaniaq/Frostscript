using Frostscript.Domain.Features.Models;
using Frostscript.Domain.Validator;

namespace Frostscript.Domain.Features
{
    public interface IFeature
    {
        public ParseResult Parse(Token[] tokens);
        public ValidationResult Validate(INode node, IDictionary<string, VariableData> variables);
    }
}
