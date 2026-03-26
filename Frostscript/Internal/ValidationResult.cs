using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Internal
{
    internal interface IValidationResult;

    internal record Pass(INode Node) : IValidationResult;
    internal record Fail(string Error, Token Token) : IValidationResult;
}
