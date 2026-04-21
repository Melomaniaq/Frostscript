using Frostscript.Domain.Features.Models;

namespace Frostscript.Domain.Parser
{
    public record AnnotationSuccess(IDataType DataType, Token[] RemainingTokens);
}
