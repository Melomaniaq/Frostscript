using Frostscript.Domain.Features.Models;

namespace Frostscript.Domain.Parser
{
    public record ParseSuccess(INode Node, Token[] RemainingTokens);
}
