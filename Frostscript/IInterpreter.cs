using Frostscript.Domain.Features.Models;

namespace Frostscript
{
    public interface IInterpreter
    {
        public void Interpret(IExpression[] ast);
    }
}
