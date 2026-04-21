using Frostscript.Domain.Features.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Frostscript.Domain.Validator
{
    public record VariableData(IDataType DataType, bool Mutable);
}
