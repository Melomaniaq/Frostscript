using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Internal
{
    internal record ParserResult(INode Node, Token[] RemainingTokens)
    {
    };
}
